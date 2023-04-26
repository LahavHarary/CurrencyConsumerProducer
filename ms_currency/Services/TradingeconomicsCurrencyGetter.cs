using shared_resources.Models;
using shared_resources.Services;

namespace ms_currency.Services;

public class TradingeconomicsCurrencyGetter : AbstractCurrencyGetter
{
    private string _prefix;
    private string _suffix;

    /// <summary>
    ///  The purpose of this CTOR is to decide from where the web scarping should happen.
    ///  The user can call this CTOR with a parameter named: useTestFile which determines if we will use
    ///  a local file or scrape Tradingeconomics for their currency data.
    /// </summary>
    /// <param name="useTestFile"></param>
    public TradingeconomicsCurrencyGetter(bool useTestFile = true)
    {
        _prefix = "//tr[@data-symbol='";
        _suffix = ":CUR']/td[@id='p']/text()";
        
        if(useTestFile)
            LoadHtmlDocument(ConfigHelperService.GetData("TestUrl"));
        else
            LoadHtmlDocument(ConfigHelperService.GetData("TradingeconomicsUrl"));
    }
    
    /// <summary>
    /// A function which is implemented from AbstractCurrencyGetter.
    /// For each values of CurrenciesRateModel a function is being called.
    /// </summary>
    /// <returns></returns>
    public override CurrenciesRateModel GetCurrenciesRateModel()
    {
        return new CurrenciesRateModel()
        {
            UsdIls = GetUsdIls(),
            EurGbp = GetEurGbp(),
            EurJpy = GetEurJpy(),
            EurUsd = GetEurUsd(),
            TimeOfReceivedData = _dateTime
        };
    }
    
    protected override double GetUsdIls()
    {
        return GeneralGetFunction("USDILS");
    }

    protected override double GetEurGbp()
    {
        return GeneralGetFunction("EURGBP");
    }

    protected override double GetEurJpy()
    {
        return GeneralGetFunction("EURJPY");
    }

    protected override double GetEurUsd()
    {
        return GeneralGetFunction("EURUSD");
    }

    /// <summary>
    /// Each one of the other get functions is calling this method.
    /// It combines the prefix with the currency identifier and the suffix, gets the relevant html
    /// and trims the string in order for us to convert it to a number.
    /// </summary>
    /// <param name="currencyIdentifier"></param>
    /// <returns></returns>
    private double GeneralGetFunction(string currencyIdentifier)
    {
        var currencyValue = _document.DocumentNode.SelectSingleNode(_prefix +currencyIdentifier +_suffix).InnerHtml;
        var trimmed = currencyValue.Substring(0, currencyValue.Length - 1);
        return Convert.ToDouble(trimmed);
    }
    
}