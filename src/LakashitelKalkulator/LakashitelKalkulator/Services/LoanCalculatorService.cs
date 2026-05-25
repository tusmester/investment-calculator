using LakashitelKalkulator.Models;

namespace LakashitelKalkulator.Services;

public interface ILoanCalculatorService
{
    CalculationResult Calculate(CalculatorInputs inputs);
}

public class LoanCalculatorService(ILogger<LoanCalculatorService> logger) : ILoanCalculatorService
{
    public CalculationResult Calculate(CalculatorInputs inputs)
    {
        logger.LogTrace("Starting calculation for house price: {HousePrice}", inputs.HousePrice);

        var result = new CalculationResult();

        // Calculate loan details
        var downPayment = inputs.HousePrice * inputs.DownPaymentPercentage / 100;
        var loanAmount = inputs.HousePrice - downPayment;
        var totalInitialInvestment = downPayment + inputs.AdditionalInitialCosts;

        var monthlyInterestRate = inputs.LoanInterestRate / 100 / 12;
        var numberOfPayments = inputs.LoanDurationYears * 12;

        // Calculate monthly payment using amortization formula
        // "What fixed payment, made every month with compound interest, will reduce
        // the loan to zero after exactly n payments?"
        var monthlyPayment = loanAmount * monthlyInterestRate * 
            (decimal)Math.Pow((double)(1 + monthlyInterestRate), (double)numberOfPayments) / 
            ((decimal)Math.Pow((double)(1 + monthlyInterestRate), (double)numberOfPayments) - 1);

        result.MonthlyLoanPayment = monthlyPayment;

        // Calculate home ownership scenario
        var remainingLoan = loanAmount;
        var totalInterestPaid = 0m;
        var investmentValue = totalInitialInvestment; // Start investment with same initial amount
        var rentSurplusInvestment = 0m; // Track invested rent surplus for property path
        var currentMonthlyRent = inputs.InitialMonthlyRent;
        var monthlyInvestmentReturnRate = inputs.EstimatedInvestmentGainsPercentage / 100 / 12;
        var cumulativeAmortization = 0m;

        // Add year 0 to show initial state
        result.YearlyBreakdowns.Add(new YearlyBreakdown
        {
            Year = 0,
            HomeValue = inputs.HousePrice,
            RemainingLoanBalance = loanAmount,
            HomeEquity = downPayment,
            InvestmentValue = totalInitialInvestment,
            YearlyRentIncome = 0,
            YearlyInvestmentContribution = 0
        });

        for (int year = 1; year <= inputs.LoanDurationYears; year++)
        {
            var yearlyBreakdown = new YearlyBreakdown
            {
                Year = year,             // Calculate home value with appreciation
                HomeValue = inputs.HousePrice *
                    (decimal)Math.Pow(1 + (double)inputs.EstimatedHomePriceYearlyIncrease / 100, year)
            };

            // Process monthly payments for this year
            decimal yearlyInterest = 0;
            decimal yearlyPrincipal = 0;
            decimal yearlyInvestmentContribution = 0;
            decimal yearStartInvestmentValue = investmentValue;

            // Calculate monthly amortization for this year (based on current year's home value)
            var monthlyAmortization = (yearlyBreakdown.HomeValue * inputs.YearlyAmortizationPercentage / 100) / 12;

            for (int month = 1; month <= 12; month++)
            {
                // Loan payment processing
                var interestPayment = remainingLoan * monthlyInterestRate;
                var principalPayment = monthlyPayment - interestPayment;

                yearlyInterest += interestPayment;
                yearlyPrincipal += principalPayment;

                remainingLoan -= principalPayment;

                if (remainingLoan < 0) remainingLoan = 0;

                // === PROPERTY PATH: Invest rent surplus ===
                // Calculate monthly rent surplus after expenses
                var monthlyRentAfterTax = currentMonthlyRent * (1 - inputs.YearlyRentTax / 100);
                var monthlyPropertyCosts = monthlyPayment + (inputs.OtherFixedHomeCostsPerYear / 12);
                var monthlyRentSurplus = monthlyRentAfterTax - monthlyPropertyCosts;

                // Apply investment return to existing surplus investments
                rentSurplusInvestment = rentSurplusInvestment * (1 + monthlyInvestmentReturnRate);

                // If there's surplus rent, invest it
                rentSurplusInvestment += monthlyRentSurplus;

                // === ALTERNATIVE INVESTMENT PATH ===
                // Apply monthly investment return first
                investmentValue = investmentValue * (1 + monthlyInvestmentReturnRate);

                // Then add monthly contributions (money saved by NOT buying the investment property):
                // What you save: loan payment + other costs + amortization (depreciation you avoid)
                // What you lose: rent income (after tax)
                // Note: If contribution would be negative (lost rent > avoided costs), we invest 0 instead
                var monthlyContribution = Math.Max(0, monthlyPayment + (inputs.OtherFixedHomeCostsPerYear / 12) + monthlyAmortization - monthlyRentAfterTax);

                investmentValue += monthlyContribution;
                yearlyInvestmentContribution += monthlyContribution;
            }

            totalInterestPaid += yearlyInterest;

            // Calculate rent income (net of tax)
            yearlyBreakdown.YearlyRentIncome = currentMonthlyRent * 12;
            var netRentIncome = yearlyBreakdown.YearlyRentIncome * (1 - inputs.YearlyRentTax / 100);
            yearlyBreakdown.YearlyRentAfterTax = netRentIncome;

            // Calculate amortization
            var yearlyAmortization = yearlyBreakdown.HomeValue * inputs.YearlyAmortizationPercentage / 100;
            yearlyBreakdown.YearlyAmortization = yearlyAmortization;
            cumulativeAmortization += yearlyAmortization;

            // Calculate rent surplus (rent after tax minus loan payment and other costs)
            var yearlyPropertyCosts = monthlyPayment * 12 + inputs.OtherFixedHomeCostsPerYear;
            yearlyBreakdown.YearlyRentSurplus = netRentIncome - yearlyPropertyCosts;

            // Update remaining loan and equity            
            yearlyBreakdown.RemainingLoanBalance = remainingLoan;

            // HomeEquity includes the cumulative amortization subtraction to match final net worth
            yearlyBreakdown.HomeEquity = yearlyBreakdown.HomeValue - remainingLoan + rentSurplusInvestment - cumulativeAmortization;

            yearlyBreakdown.YearlyInvestmentContribution = yearlyInvestmentContribution;
            yearlyBreakdown.InvestmentValue = investmentValue;

            // Update rent with inflation for next year
            currentMonthlyRent = currentMonthlyRent * (1 + inputs.EstimatedInflationPerYear / 100);

            result.YearlyBreakdowns.Add(yearlyBreakdown);
        }

        // Final calculations
        var finalBreakdown = result.YearlyBreakdowns.Last();
        result.FinalHomeValue = finalBreakdown.HomeValue;
        result.TotalLoanPayment = monthlyPayment * numberOfPayments;
        result.TotalInterestPaid = totalInterestPaid;
        result.TotalAmortization = result.YearlyBreakdowns.Sum(y => 
            y.HomeValue * inputs.YearlyAmortizationPercentage / 100);
        result.TotalOtherCosts = inputs.OtherFixedHomeCostsPerYear * inputs.LoanDurationYears;
        result.TotalRentIncome = result.YearlyBreakdowns.Sum(y => y.YearlyRentIncome);
        result.TotalRentTax = result.TotalRentIncome * inputs.YearlyRentTax / 100;

        // Net worth calculations
        // Property path: equity already includes invested rent surplus AND cumulative amortization subtraction
        result.HomeOwnershipNetWorth = finalBreakdown.HomeEquity;

        result.InvestmentNetWorth = finalBreakdown.InvestmentValue;

        result.Difference = result.HomeOwnershipNetWorth - result.InvestmentNetWorth;
        result.IsHomeOwnershipBetter = result.Difference > 0;

        logger.LogInformation("Calculation completed. Home ownership: {Home}, Investment: {Investment}", 
            result.HomeOwnershipNetWorth, result.InvestmentNetWorth);

        return result;
    }
}
