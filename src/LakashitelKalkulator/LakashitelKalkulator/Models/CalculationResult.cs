namespace LakashitelKalkulator.Models;

public class CalculationResult
{
    public decimal HomeOwnershipNetWorth { get; set; }
    public decimal InvestmentNetWorth { get; set; }
    public decimal Difference { get; set; }
    public bool IsHomeOwnershipBetter { get; set; }
    public decimal TotalLoanPayment { get; set; }
    public decimal TotalInterestPaid { get; set; }
    public decimal MonthlyLoanPayment { get; set; }
    public decimal FinalHomeValue { get; set; }
    public decimal TotalAmortization { get; set; }
    public decimal TotalOtherCosts { get; set; }
    public decimal TotalRentIncome { get; set; }
    public decimal TotalRentTax { get; set; }
    public decimal FinalSellingCosts { get; set; }
    public List<YearlyBreakdown> YearlyBreakdowns { get; set; } = new();
}

public class YearlyBreakdown
{
    public int Year { get; set; }
    public decimal HomeValue { get; set; }
    public decimal RemainingLoanBalance { get; set; }
    public decimal HomeEquity { get; set; }
    public decimal InvestmentValue { get; set; }
    public decimal YearlyRentIncome { get; set; }
    public decimal YearlyInvestmentContribution { get; set; }
    public decimal YearlyAmortization { get; set; }
    public decimal YearlyRentAfterTax { get; set; }
    public decimal YearlyRentSurplus { get; set; }
}
