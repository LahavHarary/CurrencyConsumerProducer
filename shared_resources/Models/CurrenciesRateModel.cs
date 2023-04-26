namespace shared_resources.Models;

public class CurrenciesRateModel
{
    public double UsdIls { get; set; }
    public double EurGbp { get; set; }
    public double EurJpy { get; set; }
    public double EurUsd { get; set; }

    public DateTime TimeOfReceivedData { get; set; }
}