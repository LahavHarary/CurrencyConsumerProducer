using HtmlAgilityPack;
using shared_resources.Models;

namespace ms_currency.Services;

public abstract class AbstractCurrencyGetter
{
    public HtmlDocument _document;
    protected DateTime _dateTime;
    
    public void LoadHtmlDocument(string url)
    {
        var web = new HtmlWeb();
        _document = web.Load(url);
        _dateTime = DateTime.Now;
    }

    public abstract CurrenciesRateModel GetCurrenciesRateModel();

    protected abstract double GetUsdIls();
    protected abstract double GetEurGbp();
    protected abstract double GetEurJpy();
    protected abstract double GetEurUsd();
}