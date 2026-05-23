using LakashitelKalkulator.Models;
using System.Globalization;

namespace LakashitelKalkulator.Services;

public interface ICurrencyFormatterService
{
    string Format(decimal value, Currency currency);
    string GetCurrencySymbol(Currency currency);
}

public class CurrencyFormatterService : ICurrencyFormatterService
{
    private static readonly Dictionary<Currency, CultureInfo> _cultures = new()
    {
        [Currency.HUF] = new CultureInfo("hu-HU"),
        [Currency.EUR] = new CultureInfo("de-DE"),
        [Currency.USD] = new CultureInfo("en-US"),
        [Currency.GBP] = new CultureInfo("en-GB")
    };

    public string Format(decimal value, Currency currency)
    {
        var culture = _cultures[currency];
        return value.ToString("N0", culture);
    }

    public string GetCurrencySymbol(Currency currency)
    {
        return currency switch
        {
            Currency.HUF => "Ft",
            Currency.EUR => "€",
            Currency.USD => "$",
            Currency.GBP => "£",
            _ => ""
        };
    }
}
