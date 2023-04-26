using shared_resources.Models;

namespace ms_currency.Interfaces;

public interface ICurrencyDbWriter
{
    public void Write(CurrenciesRateModel currenciesRateModel);
}