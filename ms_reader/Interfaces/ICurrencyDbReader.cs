using shared_resources.Models;

namespace ms_reader.Interfaces;

public interface ICurrencyDbReader
{
    public CurrenciesRateModel Read();
}