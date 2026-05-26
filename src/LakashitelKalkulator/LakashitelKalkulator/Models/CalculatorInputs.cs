namespace LakashitelKalkulator.Models;

public class CalculatorInputs
{
    public decimal HousePrice { get; set; } = 25_000_000;
    public decimal DownPaymentPercentage { get; set; } = 20;
    public decimal AdditionalInitialCosts { get; set; } = 4_000_000;
    public int LoanDurationYears { get; set; } = 20;
    public decimal LoanInterestRate { get; set; } = 3;
    public decimal YearlyAmortizationPercentage { get; set; } = 1.5m;
    public decimal EstimatedHomePriceYearlyIncrease { get; set; } = 5;
    public decimal EstimatedInvestmentGainsPercentage { get; set; } = 9;
    public decimal YearlyRentTax { get; set; } = 15;
    public decimal OtherFixedHomeCostsPerYear { get; set; } = 120_000;
    public decimal EstimatedInflationPerYear { get; set; } = 3.8m;
    public decimal InitialMonthlyRent { get; set; } = 150_000;
    public int InitialEmptyMonths { get; set; } = 4;
    public int EmptyMonthsPerYear { get; set; } = 1;
    public decimal FinalSellingCostsPercentage { get; set; } = 4;
    public Currency Currency { get; set; } = Currency.HUF;
}

public enum Currency
{
    HUF,
    EUR,
    USD,
    GBP
}
