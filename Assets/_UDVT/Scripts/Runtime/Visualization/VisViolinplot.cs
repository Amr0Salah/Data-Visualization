using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;
using static KernelDensityEstimation;

public class VisViolinplot : Vis
{
    public double[,] KDEresult = null;

    public VisViolinplot()
    {
        title = "ViolinPlot";

        //Define Data Mark and Tick Prefab
        dataMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Sphere");
        tickMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/VisContainer/Tick");
    }

    public override GameObject CreateVis(GameObject container)
    {
        base.CreateVis(container);

        double bandWith = CalculationHelper.CalculateBandWithByRuleOfThumb(dataSets[0].ElementAt(0).Value);
        KDEresult = KernelDensityEstimation.KDE(dataSets[0].ElementAt(0).Value, bandWith, CurrentParams.kdeStepsValue);
        ChangeDataMarks();

        StatisticalCalculations statisticalCalculations = new StatisticalCalculations(dataSets[0].ElementAt(0).Value);

        var median = statisticalCalculations.median;
        var iqr = statisticalCalculations.iqr;

        var minimum = statisticalCalculations.minimum;
        var maximum = statisticalCalculations.maximum;

        var lowerBound = statisticalCalculations.lowerBound;
        var upperBound = statisticalCalculations.upperBound; 

        var outliers = statisticalCalculations.outliers;
        var quartiles = statisticalCalculations.quartiles;

        //## 01:  Create Axes and Grids
        // X Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(0).Key, dataSets[0].ElementAt(0).Value, Direction.X);
        visContainer.CreateGrid(Direction.X, Direction.Y);

        // Y Axis
        visContainer.CreateAxis("Violin function", dataSets[0].ElementAt(1).Value, Direction.Y);

        //## 02: Set Remaining Vis Channels (Color,...)
        visContainer.SetChannel(VisChannel.XPos, dataSets[0].ElementAt(0).Value);
        visContainer.SetChannel(VisChannel.YPos, dataSets[0].ElementAt(1).Value);
        visContainer.SetChannel(VisChannel.Color, dataSets[0].ElementAt(1).Value);

        //## 03: Draw all Data Points with the provided Channels 
        visContainer.CreateDataMarks(dataMarkPrefab);

        //## 04: Rescale Chart
        visContainerObject.transform.localScale = new Vector3(width, height, depth);


        return visContainerObject;
    }

    /// <summary>
    /// Each child class can define how the Data Marks in the visualization can be changed.
    /// </summary>
    public override void ChangeDataMarks()
    {
        var xName = dataSets[0].ElementAt(0).Key;
        var yName = dataSets[0].ElementAt(1).Key;

        dataSets[0][xName] = Enumerable.Range(0, KDEresult.GetLength(0)).Select(x => KDEresult[x, 0]).ToArray();
        dataSets[0][yName] = Enumerable.Range(0, KDEresult.GetLength(0)).Select(x => KDEresult[x, 1]).ToArray();
    }
}
