# Lakáshitel Kalkulátor / Home Loan Calculator

An application for comparing home ownership vs investment strategies.

## Features

### Calculator Inputs
- **House Price**: Current purchase price of the property
- **Down Payment Percentage**: Initial payment percentage (default: 20%)
- **Additional Initial Costs**: Renovation, fees, legal costs (default: 4,000,000 HUF)
- **Loan Duration**: Years to repay the loan (default: 20 years)
- **Loan Interest Rate**: Fixed annual interest rate (default: 3%)
- **Yearly Amortization**: Annual property depreciation (default: 1.2%)
- **Estimated Home Price Increase**: Expected annual property appreciation (default: 7%)
- **Estimated Investment Gains**: Expected annual investment returns (default: 7%)
- **Rent Income Tax**: Personal income tax on rental income (default: 15%)
- **Other Fixed Home Costs**: Annual maintenance costs (default: 120,000 HUF)
- **Estimated Inflation**: Annual inflation rate (default: 3.8%)
- **Initial Monthly Rent**: Starting rent that increases with inflation (default: 160,000 HUF)

### Comparison Logic

#### Home Ownership Path
1. Buy house with down payment and loan
2. Pay monthly loan installments (capital + interest)
3. Rent out the property for monthly income
4. Pay taxes on rental income
5. House value appreciates annually
6. Property depreciates (amortization)
7. Pay other fixed costs (maintenance, utilities, insurance)
8. Net worth = Home equity + net rent income - costs

#### Investment Path
1. Invest the down payment amount
2. Each month invest: loan payment + other costs + interest portion - rent received
3. Investment grows at estimated return rate
4. Net worth = total investment value

### Features
- **Bilingual UI**: Hungarian (default) and English
- **Detailed yearly breakdown**: Track equity and investment growth year by year
- **Comprehensive results**: See winner, monthly payments, total costs, and final values
- **Tooltips and help texts**: Understand each parameter
- **Responsive design**: Works on desktop and mobile

## Architecture

### Services
- **LoanCalculatorService**: Core calculation logic with amortization schedule
- **LocalizationService**: Multi-language support
- **CurrencyFormatterService**: Currency formatting by locale

### Models
- **CalculatorInputs**: All input parameters
- **CalculationResult**: Comprehensive results with yearly breakdowns
- **Currency**: Supported currencies enum

## Running the Application

```bash
dotnet run
```

Navigate to https://localhost:5001 (or the displayed URL) to use the calculator.

## Technology Stack
- .NET 10
- Blazor Server with Interactive render mode
- Bootstrap 5 for styling
