using Microsoft.Extensions.Localization;

namespace LakashitelKalkulator.Services;

public class LocalizationService : IStringLocalizer
{
    private readonly ILogger<LocalizationService> _logger;
    private Dictionary<string, string> _currentStrings;

    public string CurrentLanguage { get; private set; } = "hu";
    public List<string> AvailableLanguages { get; } = ["hu", "en"];

    private static readonly Dictionary<string, Dictionary<string, string>> _resources = new()
    {
        ["hu"] = new Dictionary<string, string>
        {
            ["AppTitle"] = "Befektetési Ingatlan Kalkulátor",
            ["CompareInvestments"] = "Befektetési ingatlan vs. Alternatív befektetés",
            ["PurchaseAndLoan"] = "Vásárlás és Hitel",
            ["OperatingAndReturns"] = "Működés és Hozamok",
            ["HousePrice"] = "Ingatlan ára",
            ["HousePriceTooltip"] = "A befektetési ingatlan jelenlegi vételára",
            ["DownPaymentPercentage"] = "Önerő százaléka",
            ["DownPaymentPercentageTooltip"] = "A vételár hány százalékát fizeted ki azonnal",
            ["AdditionalCosts"] = "Egyéb kezdeti költségek",
            ["AdditionalCostsTooltip"] = "Felújítás, illetékek, ügyvédi költségek",
            ["LoanDuration"] = "Hitel futamideje (év)",
            ["LoanDurationTooltip"] = "Hány év alatt törlesztesz",
            ["LoanInterest"] = "Hitel kamata (%)",
            ["LoanInterestTooltip"] = "Fix éves kamatláb",
            ["YearlyAmortization"] = "Éves amortizáció (%)",
            ["YearlyAmortizationTooltip"] = "Az ingatlan értékcsökkenése évente (épület elhasználódása)",
            ["EstimatedHomeIncrease"] = "Ingatlan éves áremelkedés (%)",
            ["EstimatedHomeIncreaseTooltip"] = "Várható éves ingatlanár növekedés",
            ["EstimatedInvestmentGains"] = "Befektetés éves hozama (%)",
            ["EstimatedInvestmentGainsTooltip"] = "Várható éves hozam alternatív befektetésen (részvény, kötvény, stb.)",
            ["RentTax"] = "Bérleti díj adója (%)",
            ["RentTaxTooltip"] = "Személyi jövedelemadó a bérleti díj után",
            ["OtherCosts"] = "Egyéb éves költségek",
            ["OtherCostsTooltip"] = "Közös költség, biztosítás az ingatlanra, minden, amit nem a bérlő fizet",
            ["Inflation"] = "Várható éves infláció (%)",
            ["InflationTooltip"] = "Az infláció mértéke, ezzel nő a bérleti díj amit kérsz",
            ["InitialRent"] = "Kezdeti havi bérleti díj",
            ["InitialRentTooltip"] = "Havi bérleti díj amit kapsz a bérlőtől, évente inflációval nő",
            ["Currency"] = "Pénznem",
            ["Calculate"] = "Számol",
            ["Results"] = "Eredmények",
            ["HomeOwnership"] = "Befektetési Ingatlan",
            ["Investment"] = "Alternatív Befektetés",
            ["Winner"] = "Győztes",
            ["Difference"] = "Különbség",
            ["MonthlyPayment"] = "Havi törlesztő",
            ["TotalInterest"] = "Összes kamat",
            ["FinalHomeValue"] = "Ingatlan végső értéke",
            ["TotalAmortization"] = "Összes amortizáció",
            ["TotalOtherCosts"] = "Összes egyéb költség",
            ["TotalRentIncome"] = "Összes bérleti díj bevétel",
            ["TotalRentTax"] = "Összes bérleti díj adó",
            ["NetWorth"] = "Nettó vagyon",
            ["YearlyBreakdown"] = "Éves bontás",
            ["Year"] = "Év",
            ["HomeValue"] = "Ingatlan értéke",
            ["RemainingLoan"] = "Fennálló hitel",
            ["HomeEquity"] = "Saját tőke",
            ["InvestmentValue"] = "Befektetés értéke",
            ["YearlyRentIncome"] = "Éves bérleti díj",
            ["YearlyAmortization"] = "Éves amortizáció",
            ["YearlyRentAfterTax"] = "Adózott bérleti díj",
            ["YearlyRentSurplus"] = "Bérleti többlet",
            ["Language"] = "Nyelv",
            ["Years"] = "év",
            ["NoResultsYet"] = "Nincs még eredmény",
            ["FillFieldsMessage"] = "Töltsd ki a mezőket és kattints a Számol gombra",
            ["ViewYearlyBreakdown"] = "Részletes éves bontás",
            ["HowItWorks"] = "Hogyan működik?",
            ["PropertyPathTitle"] = "Befektetési Ingatlan Út",
            ["PropertyPathDesc"] = "Ebben a forgatókönyvben megvásárolsz egy befektetési ingatlant, amit bérbeadsz.",
            ["PropertyInitialCosts"] = "Kezdeti költségek: Önerő + egyéb kezdeti költségek (felújítás, illetékek)",
            ["PropertyMonthlyIncome"] = "Havi bevétel: Bérleti díj (adózás után)",
            ["PropertyMonthlyCosts"] = "Havi költségek: Hitel törlesztő + egyéb költségek (közös, biztosítás)",
            ["PropertyRentSurplus"] = "Bérleti többlet: Ha a bevétel meghaladja a költségeket, a különbözetet befektetjük",
            ["PropertyAppreciation"] = "Ingatlan értéknövekedés: Az ingatlan értéke évente nő",
            ["PropertyDepreciation"] = "Amortizáció: Az épület elhasználódása csökkenti az értéket",
            ["PropertyFinalValue"] = "Végső érték: Ingatlan értéke - amortizáció + befektetett bérleti többlet",
            ["InvestmentPathTitle"] = "Alternatív Befektetési Út",
            ["InvestmentPathDesc"] = "Ebben a forgatókönyvben nem vásárolsz ingatlant, hanem befektetsz (pl. részvény, kötvény).",
            ["InvestmentInitialAmount"] = "Kezdő tőke: Ugyanannyi, mint az ingatlan esetén (önerő + egyéb költségek)",
            ["InvestmentMonthlyContribution"] = "Havi befektetés: A megtakarított költségek (törlesztő + egyéb + amortizáció) - elveszített bérleti díj",
            ["InvestmentNoNegative"] = "Ha a befektetés negatív lenne (bérleti díj > megtakarított költségek), akkor 0-t fektetünk be",
            ["InvestmentCompoundGrowth"] = "Kamatos kamat: A befektetés havonta kamatozik a becsült éves befektetési hozam szerint",
            ["InvestmentFinalValue"] = "Végső érték: Befektetés kamatos kamattal + havi befektetések"
        },
        ["en"] = new Dictionary<string, string>
        {
            ["AppTitle"] = "Investment Property Calculator",
            ["CompareInvestments"] = "Investment Property vs. Alternative Investment",
            ["PurchaseAndLoan"] = "Purchase & Loan",
            ["OperatingAndReturns"] = "Operating & Returns",
            ["HousePrice"] = "Property Price",
            ["HousePriceTooltip"] = "Current purchase price of the investment property",
            ["DownPaymentPercentage"] = "Down Payment Percentage",
            ["DownPaymentPercentageTooltip"] = "What percentage of the purchase price you pay upfront",
            ["AdditionalCosts"] = "Additional Initial Costs",
            ["AdditionalCostsTooltip"] = "Renovation, fees, legal costs",
            ["LoanDuration"] = "Loan Duration (years)",
            ["LoanDurationTooltip"] = "How many years to repay",
            ["LoanInterest"] = "Loan Interest Rate (%)",
            ["LoanInterestTooltip"] = "Fixed annual interest rate",
            ["YearlyAmortization"] = "Yearly Amortization (%)",
            ["YearlyAmortizationTooltip"] = "Annual depreciation of the property (building wear)",
            ["EstimatedHomeIncrease"] = "Estimated Property Price Increase (%)",
            ["EstimatedHomeIncreaseTooltip"] = "Expected annual property price growth",
            ["EstimatedInvestmentGains"] = "Estimated Investment Gains (%)",
            ["EstimatedInvestmentGainsTooltip"] = "Expected annual return on alternative investment (stocks, bonds, etc.)",
            ["RentTax"] = "Rent Income Tax (%)",
            ["RentTaxTooltip"] = "Personal income tax on rental income you receive from tenant",
            ["OtherCosts"] = "Other Annual Costs",
            ["OtherCostsTooltip"] = "Common costs, utilities, insurance for the property",
            ["Inflation"] = "Expected Annual Inflation (%)",
            ["InflationTooltip"] = "Inflation rate, rent you charge increases by this amount",
            ["InitialRent"] = "Initial Monthly Rent",
            ["InitialRentTooltip"] = "Monthly rent you receive from tenant, increases with inflation annually",
            ["Currency"] = "Currency",
            ["Calculate"] = "Calculate",
            ["Results"] = "Results",
            ["HomeOwnership"] = "Investment Property",
            ["Investment"] = "Alternative Investment",
            ["Winner"] = "Winner",
            ["Difference"] = "Difference",
            ["MonthlyPayment"] = "Monthly Payment",
            ["TotalInterest"] = "Total Interest",
            ["FinalHomeValue"] = "Final Property Value",
            ["TotalAmortization"] = "Total Amortization",
            ["TotalOtherCosts"] = "Total Other Costs",
            ["TotalRentIncome"] = "Total Rent Income",
            ["TotalRentTax"] = "Total Rent Tax",
            ["NetWorth"] = "Net Worth",
            ["YearlyBreakdown"] = "Yearly Breakdown",
            ["Year"] = "Year",
            ["HomeValue"] = "Property Value",
            ["RemainingLoan"] = "Remaining Loan",
            ["HomeEquity"] = "Property Equity",
            ["InvestmentValue"] = "Investment Value",
            ["YearlyRentIncome"] = "Yearly Rent Income",
            ["YearlyAmortization"] = "Yearly Amortization",
            ["YearlyRentAfterTax"] = "Rent After Tax",
            ["YearlyRentSurplus"] = "Rent Surplus",
            ["Language"] = "Language",
            ["Years"] = "years",
            ["NoResultsYet"] = "No Results Yet",
            ["FillFieldsMessage"] = "Fill in the fields and click Calculate",
            ["ViewYearlyBreakdown"] = "View Yearly Breakdown",
            ["HowItWorks"] = "How It Works?",
            ["PropertyPathTitle"] = "Investment Property Path",
            ["PropertyPathDesc"] = "In this scenario, you purchase an investment property and rent it out.",
            ["PropertyInitialCosts"] = "Initial costs: Down payment + additional costs (renovation, fees)",
            ["PropertyMonthlyIncome"] = "Monthly income: Rental income (after tax)",
            ["PropertyMonthlyCosts"] = "Monthly costs: Loan payment + other costs (common, insurance)",
            ["PropertyRentSurplus"] = "Rent surplus: If income exceeds costs, the difference is invested",
            ["PropertyAppreciation"] = "Property appreciation: Property value increases annually",
            ["PropertyDepreciation"] = "Amortization: Building wear and tear decreases value",
            ["PropertyFinalValue"] = "Final value: Property value - amortization + invested rent surplus",
            ["InvestmentPathTitle"] = "Alternative Investment Path",
            ["InvestmentPathDesc"] = "In this scenario, you don't buy property but invest instead (e.g., stocks, bonds).",
            ["InvestmentInitialAmount"] = "Initial capital: Same as property scenario (down payment + other costs)",
            ["InvestmentMonthlyContribution"] = "Monthly investment: Saved costs (loan payment + other + amortization) - lost rental income",
            ["InvestmentNoNegative"] = "If investment would be negative (rent > saved costs), we invest 0",
            ["InvestmentCompoundGrowth"] = "Compound interest: Investment grows monthly according to estimated investment gains",
            ["InvestmentFinalValue"] = "Final value: Investment with compound interest + monthly contributions"
        }
    };

    public LocalizationService(ILogger<LocalizationService> logger)
    {
        _logger = logger;
        _currentStrings = _resources[CurrentLanguage];
    }

    // IStringLocalizer implementation
    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value, resourceNotFound: value == name);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var format = GetString(name);
            var value = string.Format(format, arguments);
            return new LocalizedString(name, value, resourceNotFound: format == name);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        return _currentStrings.Select(kvp => new LocalizedString(kvp.Key, kvp.Value, false));
    }

    // Custom methods (kept for convenience)
    public string GetString(string key)
    {
        if (_currentStrings.TryGetValue(key, out var value))
        {
            return value;
        }
        _logger.LogWarning("Missing localization key: {Key}", key);
        return key;
    }

    public void SetLanguage(string language)
    {
        if (_resources.TryGetValue(language, out Dictionary<string, string>? value))
        {
            CurrentLanguage = language;
            _currentStrings = value;
            _logger.LogInformation("Language changed to: {Language}", language);
        }
    }
}
