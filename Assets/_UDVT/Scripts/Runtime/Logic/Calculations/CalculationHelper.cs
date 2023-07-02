using System;
using System.Linq;

public static class CalculationHelper
{
    public static double CalculateMean(double[] numbers)
    {
        double sum = 0;
        foreach (var number in numbers)
        {
            sum += number;
        }
        return sum / numbers.Length;
    }

    public static double CalculateVariance(double[] numbers, double mean)
    {
        double sumSquaredDifference = 0;
        foreach (var number in numbers)
        {
            double difference = number - mean;
            sumSquaredDifference += difference * difference;
        }
        return sumSquaredDifference / numbers.Length;
    }

    public static double CalculateStandartVariance(double[] numbers)
    {
        double mean = CalculateMean(numbers);
        double variance = CalculateVariance(numbers, mean);
        return Math.Sqrt(variance);
    }

    public static double CalculateBandWithByRuleOfThumb(double[] numbers)
    {
        double standartVariance = CalculateStandartVariance(numbers);
        int dataSize = numbers.Length;
        double sigma = 1.6 * standartVariance * Math.Pow((double)dataSize, -1.0 / 5.0);
        return sigma;
    }
}
