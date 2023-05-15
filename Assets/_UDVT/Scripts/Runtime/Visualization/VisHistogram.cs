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

        //## 01:  Create Axes and Grids


        /// Drow Plot but i didn't make the range of the drow true 

        double[] Xarr = dataSets[0].ElementAt(0).Value;
        int Xaxis = Xarr.Length;
        double max = Xarr.Max();
        double min = Xarr.Min();
        Debug.Log(max);

        Debug.Log(min);

        int bin = 20

        double binRange = max / bin;
        Debug.Log(binRange);

        double total1 = 0;

        double[] X_arr = new double[bin];
        double[] D_arr = new double[bin];

        for (int i = 0; i < bin; i++)
        {
            D_arr[i] = 0;
            X_arr[i] = i * binRange;

        }

        for (int i = 0; i < Xaxis; i++)
        {
            for (int j = 0; j < bin; j++)
            {
                if (((j - 1) * binRange) <= Xarr[i] && (j * binRange) > Xarr[i])
                {
                    D_arr[j]++;
                    total1++;
                    break;
                }

            }


        }
        double total = 0;
        for (int i = 0; i < bin; i++)
        {
            i++;
        }


        // X Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(0).Key, dataSets[0].ElementAt(0).Value, Direction.X);
        visContainer.CreateGrid(Direction.X, Direction.Y);

        // Y Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(1).Key, dataSets[0].ElementAt(1).Value, Direction.Y);

        // Z Axis
        //visContainer.CreateAxis(dataSets[0].ElementAt(2).Key, dataSets[0].ElementAt(2).Value, Direction.Z);
        //visContainer.CreateGrid(Direction.Y, Direction.Z);
        //visContainer.CreateGrid(Direction.Z, Direction.X);

        //## 02: Set Remaining Vis Channels (Color,...)

        visContainer.SetChannel(VisChannel.XPos, X_arr);
        visContainer.SetChannel(VisChannel.YSize, D_arr);

        //visContainer.SetChannel(VisChannel.XPos, dataSets[0].ElementAt(0).Value);
        //visContainer.SetChannel(VisChannel.YSize, dataSets[0].ElementAt(1).Value);
        // visContainer.SetChannel(VisChannel.ZPos, dataSets[0].ElementAt(2).Value);
        visContainer.SetChannel(VisChannel.Color, dataSets[0].ElementAt(3).Value);

        //## 03: Draw all Data Points with the provided Channels 
        visContainer.CreateDataMarks(dataMarkPrefab);

        //## 04: Rescale Chart
        visContainerObject.transform.localScale = new Vector3(width, height, depth);

        return visContainerObject;
    }
}
