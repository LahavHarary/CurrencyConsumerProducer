using shared_resources.Models;

namespace ms_reader.Services;

public static class PresentationService
{
    public static void Present(CurrenciesRateModel currenciesRateModel)
    {
        Console.WriteLine($"Time of report: {currenciesRateModel.TimeOfReceivedData}");
        Console.WriteLine($"EurGbp : {currenciesRateModel.EurGbp}");
        Console.WriteLine($"EurJpy : {currenciesRateModel.EurJpy}");
        Console.WriteLine($"EurUsd : {currenciesRateModel.EurUsd}");
        Console.WriteLine($"UsdIls : {currenciesRateModel.UsdIls}");
    }
}