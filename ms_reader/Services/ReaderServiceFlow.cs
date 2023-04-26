namespace ms_reader.Services;

public static class ReaderServiceFlow
{
    public static void RunFlow(int? numberOfIterations = 10, int mSecWait = 10000)
    {
        for (int i = 0; i < numberOfIterations; i++)
        {
            try
            {
                SqlLiteCurrenciesRateReader sqlLiteCurrenciesRateReader = new SqlLiteCurrenciesRateReader();
                var currenciesRateModel = sqlLiteCurrenciesRateReader.Read();
                PresentationService.Present(currenciesRateModel);
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