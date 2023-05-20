using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisHistogram : Vis
{
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

        UpdatexyzTicks(true);
        changeData();

        //## 01:  Create Axes and Grids

        // X Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(0).Key, dataSets[0].ElementAt(0).Value, Direction.X);
        visContainer.CreateGrid(Direction.X, Direction.Y);

        // Y Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(1).Key, dataSets[0].ElementAt(1).Value, Direction.Y);


        //## 02: Set Remaining Vis Channels (Color,...)

        visContainer.SetChannel(VisChannel.XPos, dataSets[0].ElementAt(0).Value);
        visContainer.SetChannel(VisChannel.YSize, dataSets[0].ElementAt(1).Value);

        // visContainer.SetChannel(VisChannel.ZPos, dataSets[0].ElementAt(2).Value);
        visContainer.SetChannel(VisChannel.Color, dataSets[0].ElementAt(3).Value);

        //## 03: Draw all Data Points with the provided Channels 
        visContainer.CreateDataMarks(dataMarkPrefab);

        //## 04: Rescale Chart
        visContainerObject.transform.localScale = new Vector3(width, height, depth);

        return visContainerObject;
    }

    #region private

    private void UpdatexyzTicks(bool useSquareRoot)
    {
        xyzTicks[0] = useSquareRoot
                        ? (int)Math.Round(Math.Sqrt(dataSets[0].ElementAt(0).Value.Length)) + 1 //Square-root choice
                        : (int)Math.Round(Math.Log(dataSets[0].ElementAt(0).Value.Length, 2)) + 1; //Sturges' formula
    }

    private void changeData()
    {
        double[] minMaxValues = dataSets[0].ElementAt(0).Value;

        double[] _x = new double[xyzTicks[0]];
        double _min = minMaxValues.Min();
        double _max = minMaxValues.Max();
        double _range = (_max - _min) / xyzTicks[0];
        
        int numberOfMaxRepeatedNumber = 0;
        List<double> frequency = new List<double>();

        for (var i = 0;i< xyzTicks[0]; i++)
        {
            int temp = minMaxValues.Where(q => (q >= _min) && (q < (_min + _range))).Count();
            frequency.Add(temp);

            _min = _min + _range;
            _x[i] = _min;
        }

        updateDataset(_x, frequency.ToArray());
    }

    private void updateDataset(double[] xData, double[] yData)
    {
        var xName = dataSets[0].ElementAt(0).Key;
        var yName = dataSets[0].ElementAt(1).Key;
        dataSets[0][xName] = xData;
        dataSets[0][yName] = yData;
    }

    #endregion private
}
