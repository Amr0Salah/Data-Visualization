using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// It contains the formulas needed to calculate binings.
/// </summary>
public enum BinningType
{
    Squareroot,
    Sturges
}

/// <summary>
/// It contains the necessary methods and parameters for ploting histograms.
/// </summary>
public class VisHistogram : Vis
{
    private List<double> _xData = new List<double>();
    private List<double> _frequency = new List<double>();

    public VisHistogram()
    {
        title = "Histogram";

        //Define Data Mark and Tick Prefab
        dataMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Cube");
        tickMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/VisContainer/Tick");
    }

    public override GameObject CreateVis(GameObject container)
    {
        base.CreateVis(container);

        UpdatexyzTicks();
        ChangeAxisAttribute(0,0, xyzTicks[0]);

        //## 01:  Create Axes and Grids
        // X Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(0).Key, dataSets[0].ElementAt(0).Value, Direction.X);
        visContainer.CreateGrid(Direction.X, Direction.Y);

        // Y Axis
        visContainer.CreateAxis("frequency", dataSets[0].ElementAt(1).Value, Direction.Y);


        //## 02: Set Remaining Vis Channels (Color,...)
        visContainer.SetChannel(VisChannel.XPos, dataSets[0].ElementAt(0).Value);
        visContainer.SetChannel(VisChannel.YSize, dataSets[0].ElementAt(1).Value);
        visContainer.SetChannel(VisChannel.Color, dataSets[0].ElementAt(3).Value);

        //## 03: Draw all Data Points with the provided Channels 
        visContainer.CreateDataMarks(dataMarkPrefab);

        //## 04: Rescale Chart
        visContainerObject.transform.localScale = new Vector3(width, height, depth);

        return visContainerObject;
    }

    /// <summary>
    /// Each child class can define how the axis attribute in the visualization can be changed.
    /// </summary>
    /// <param name="axisId"></param>
    /// <param name="selectedDimension"></param>
    /// <param name="numberOfTicks"></param>
    public override void ChangeAxisAttribute(int axisId, int selectedDimension, int numberOfTicks)
    {
        double[] minMaxValues = dataSets[0].ElementAt(selectedDimension).Value;

        double _min = minMaxValues.Min();
        double _max = minMaxValues.Max();
        double _range = (_max - _min) / numberOfTicks;

        for (var i = 0; i < numberOfTicks; i++)
        {
            int temp = minMaxValues.Where(q => (q >= _min) && (q < (_min + _range))).Count();
            _frequency.Add(temp);

            _min = _min + _range;
            _xData.Add(_min);
        }

        ChangeDataMarks();
    }

    /// <summary>
    /// Each child class can define how the Data Marks in the visualization can be changed.
    /// </summary>
    public override void ChangeDataMarks()
    {
        var xName = dataSets[0].ElementAt(0).Key;
        var yName = dataSets[0].ElementAt(1).Key;
        dataSets[0][xName] = _xData.ToArray();
        dataSets[0][yName] = _frequency.ToArray();
    }

    #region private

    /// <summary>
    /// After calculating the bining, it updates the xyzTicks.
    /// </summary>
    private void UpdatexyzTicks()
    {
        int len = dataSets[0].ElementAt(0).Value.Length;

        xyzTicks[0] = (CurrentParams.currentBinningType == BinningType.Squareroot)
            ? (int)Math.Ceiling(Math.Sqrt(len)) //Square-root choice
            : (int)Math.Ceiling(Math.Log(len, 2)) + 1; //Sturges' formula
    }

    #endregion private
}
