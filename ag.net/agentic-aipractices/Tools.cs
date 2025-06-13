using AutoGen.Core;

namespace AgenticAIPractices;

public partial class Tools
{
    [Function("UpperCase", "Converts a given string to uppercase.")]
    public async Task<string> UpperCase(string input)
    {
        await Task.Delay(10);

        return input.ToUpper();
    }

    [Function("Concat", "Concatenates an array of strings with a specified separator.")]
    public async Task<string> Concat(string[] inputStrings, string separator = ", ")
    {
        await Task.Delay(10);

        if (inputStrings == null || inputStrings.Length == 0)
        {
            return string.Empty;
        }

        return string.Join(separator, inputStrings.Select(s => s?.Trim() ?? string.Empty));
    }

    [Function("CalculateTax", "Calculates the tax on a given amount based on a specified tax rate.")]
    public async Task<string> CalculateTax(decimal amount, decimal taxRate = 0.2m)
    {
        await Task.Delay(10);

        if (amount < 0 || taxRate < 0)
        {
            throw new ArgumentException("Amount and tax rate must be non-negative.");
        }

        decimal tax = amount * taxRate;
        return $"The tax on {amount:C} at a rate of {taxRate:P} is {tax:C}.";
    }
}