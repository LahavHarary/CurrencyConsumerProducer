using System.Data.SQLite;
using ms_currency.Interfaces;
using shared_resources.Models;
using shared_resources.Services;

namespace ms_currency.Services
{
    public class SqliteCurrenciesRateWriter : ICurrencyDbWriter
    {
        private readonly string _mainDbFilename;
        private readonly string _configDirectory;
        private const string CONFIG_NAME_OF_DB_FILE = "NameOfDbFile";

        public SqliteCurrenciesRateWriter()
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
        /// This function is responsible to handle the entire writing process into the db.
        /// It first creates the first scheme which contains CurrenciesRateModel and later
        /// on creates the second scheme which contains the time that the db was updated.
        /// </summary>
        /// <param name="currenciesRateModel"></param>
        public void Write(CurrenciesRateModel currenciesRateModel)
        {
            string cs = $"URI=file:{Path.Combine(_configDirectory, _mainDbFilename)};Version=3;DefaultTimeout=30";
            try
            {
                using var con = new SQLiteConnection(cs);
                con.Open();
                using var cmd = new SQLiteCommand(con);
                // begin a transaction
                using var trans = con.BeginTransaction();

                try
                {
                    // create the currencies table
                    cmd.CommandText = "DROP TABLE IF EXISTS currencies";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"CREATE TABLE currencies(id INTEGER PRIMARY KEY,
                    CurrenciesIdentifier TEXT, rate DECIMAL)";
                    cmd.ExecuteNonQuery();

                    // insert the currencies rate data
                    cmd.CommandText =
                        $"INSERT INTO currencies(CurrenciesIdentifier, rate) VALUES('EurGbp',{currenciesRateModel.EurGbp})";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText =
                        $"INSERT INTO currencies(CurrenciesIdentifier, rate) VALUES('EurJpy',{currenciesRateModel.EurJpy})";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText =
                        $"INSERT INTO currencies(CurrenciesIdentifier, rate) VALUES('EurUsd',{currenciesRateModel.EurUsd})";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText =
                        $"INSERT INTO currencies(CurrenciesIdentifier, rate) VALUES('UsdIls',{currenciesRateModel.UsdIls})";
                    cmd.ExecuteNonQuery();

                    // commit the transaction
                    trans.Commit();

                    Console.WriteLine($"Table {_mainDbFilename} created");
                    cmd.CommandText = "DROP TABLE IF EXISTS CurrencyUpdatedTimeDb";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"CREATE TABLE CurrencyUpdatedTimeDb(id INTEGER PRIMARY KEY,
                    last_updated DATETIME)";
                    cmd.ExecuteNonQuery();

                    // insert the last updated time
                    cmd.CommandText =
                        $"INSERT INTO CurrencyUpdatedTimeDb(last_updated) VALUES('{currenciesRateModel.TimeOfReceivedData.ToString("yyyy-MM-dd HH:mm:ss")}')";
                    cmd.ExecuteNonQuery();

                    con.Close();
                }
                catch (Exception ex)
                {
                    // rollback the transaction on error
                    trans.Rollback();
                    Console.WriteLine($"Error inserting data into currencies table: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to database: {ex.Message}");
            }
        }
    }
}