using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
public class StatisticalCalculations
{
    //NewCode_12
    private double[] data;
    
    public double[] quartiles;
    public double iqr;
    public double median;
    public double minimum;
    public double maximum;
    public double lowerBound;
    public double upperBound;
    public List<double> outliers = new List<double>();

    public StatisticalCalculations(double[] dataArray)
    {
        Array.Sort(dataArray);
        data = dataArray;

        // Calculate median
        median = CalculateMedian(data);

        // Calculate IQR
        CalculateQuartiles();
        CalculateIQR();

        CalculateOutliers();
    }

    private double CalculateMedian(double[] array)
    {

        int length = array.Length;
        if (length % 2 == 0)
        {
            // Even number of elements, take average of the middle two
            int middleIndex = length / 2;
            return (array[middleIndex - 1] + array[middleIndex]) / 2.0;
        }
        else
        {
            // Odd number of elements, take the middle element
            int middleIndex = length / 2;
            return array[middleIndex];
        }
    }

    // Function to calculate quartiles of an array
    private void CalculateQuartiles()
    {
        int length = data.Length;
        int mid = length / 2;
        quartiles = new double[3];

        if (length % 2 == 0)
        {
            // Even number of elements
            quartiles[0] = CalculateMedian(data.Take(mid).ToArray()); // Lower quartile (Q1)
            quartiles[1] = CalculateMedian(data); // Median (Q2)
            quartiles[2] = CalculateMedian(data.Skip(mid).ToArray()); // Upper quartile (Q3)
        }
        else
        {
            // Odd number of elements
            quartiles[0] = CalculateMedian(data.Take(mid).ToArray()); // Lower quartile (Q1)
            quartiles[1] = CalculateMedian(data); // Median (Q2)
            quartiles[2] = CalculateMedian(data.Skip(mid + 1).ToArray()); // Upper quartile (Q3)
        }
    }

    // Function to calculate the interquartile range (IQR) of an array
    private void CalculateIQR()
    {
        iqr = quartiles[2] - quartiles[0];
    }

    private void CalculateOutliers()
    {
        // Calculate extreme points
        minimum = data[0];
        maximum = data[data.Length - 1];

        lowerBound = quartiles[0] - 1.5 * iqr;
        upperBound = quartiles[2] + 1.5 * iqr;

        foreach (double value in data)
        {
            if (value < lowerBound || value > upperBound)
            {
                outliers.Add(value);
            }
        }
    }
}
