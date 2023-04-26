using System.Data.SQLite;
using shared_resources.Models;
using ms_reader.Interfaces;
using shared_resources.Services;

namespace ms_reader.Services
{
    public class SqlLiteCurrenciesRateReader : ICurrencyDbReader
    {
        private readonly string _mainDbFilename;
        private readonly string _configDirectory;
        private const string CONFIG_NAME_OF_DB_FILE = "NameOfDbFile";

        public SqlLiteCurrenciesRateReader()
        {
            _mainDbFilename = ConfigHelperService.GetData(CONFIG_NAME_OF_DB_FILE);
            _configDirectory = ConfigHelperService.GetConfigDirectory();
            if (string.IsNullOrEmpty(_mainDbFilename))
            {
                throw new ArgumentException("Database filename is missing or invalid.");
            }

            if (string.IsNullOrEmpty(_configDirectory))
            {
                throw new ArgumentException("Configuration Directory is missing or invalid.");
            }
        }

        /// <summary>
        /// A function which handles getting the relevant data from the db and return it to the caller
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public CurrenciesRateModel Read()
        {
            string cs = $"URI=file:{Path.Combine(_configDirectory, _mainDbFilename)};Version=3;DefaultTimeout=30";
            try
            {
                using var con = new SQLiteConnection(cs);
                con.Open();
                using var cmd = new SQLiteCommand(con);

                // select the currencies rate data from the currencies table
                cmd.CommandText = "SELECT * FROM currencies";
                using var reader = cmd.ExecuteReader();

                // read the data into a CurrenciesRateModel object
                var currenciesRateModel = new CurrenciesRateModel();
                while (reader.Read())
                {
                    var currenciesIdentifier = reader.GetString(1);
                    var rate = reader.GetDecimal(2);

                    switch (currenciesIdentifier)
                    {
                        case "EurGbp":
                            currenciesRateModel.EurGbp = Convert.ToDouble(rate);
                            break;
                        case "EurJpy":
                            currenciesRateModel.EurJpy = Convert.ToDouble(rate);
                            break;
                        case "EurUsd":
                            currenciesRateModel.EurUsd = Convert.ToDouble(rate);
                            break;
                        case "UsdIls":
                            currenciesRateModel.UsdIls = Convert.ToDouble(rate);
                            break;
                        default:
                            throw new ArgumentException($"Unknown currency identifier: {currenciesIdentifier}");
                    }
                }

                // select the last updated time from the CurrencyUpdatedTimeDb table
                using var cmdTime = new SQLiteCommand(con);
                cmdTime.CommandText = "SELECT last_updated FROM CurrencyUpdatedTimeDb";
                using var timeReader = cmdTime.ExecuteReader();

                // read the last updated time
                if (timeReader.Read())
                {
                    var lastUpdated = timeReader.GetDateTime(0);
                    currenciesRateModel.TimeOfReceivedData = lastUpdated;
                }

                return currenciesRateModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading data from database: {ex.Message}");
                return null;
            }
        }
    }
}