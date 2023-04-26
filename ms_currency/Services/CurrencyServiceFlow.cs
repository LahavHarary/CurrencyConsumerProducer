using shared_resources.Models;

namespace ms_currency.Services;

public static class CurrencyServiceFlow
{
    /// <summary>
    /// A function which handles services that scrape a specific site and update db with relevant data.
    /// The function has limit of runs and sleeps after each iteration in order to prevent the site
    /// from blocking us.
    /// </summary>
    /// <param name="numberOfIterations"></param>
    /// <param name="useTestFile"></param>
    public static void RunFlow(int? numberOfIterations = 10, bool useTestFile = true, int mSecWait = 10000)
    {
        var currencyGetter = new TradingeconomicsCurrencyGetter(useTestFile);
        var currencyWriter = new SqliteCurrenciesRateWriter();

        for (int i = 0; i < numberOfIterations; i++)
        {
            try
            {
                var currenciesRateModel = currencyGetter.GetCurrenciesRateModel();
                currencyWriter.Write(currenciesRateModel);
                Console.WriteLine($"Thread is going to sleep for {mSecWait / 1000}");
                Thread.Sleep(mSecWait);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    
}