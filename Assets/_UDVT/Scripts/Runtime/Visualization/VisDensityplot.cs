using System.Linq;
using UnityEngine;
using System.Collections.Generic;

using static KernelDensityEstimation;


/// <summary>
/// It contains the necessary methods and parameters for ploting Densityplot.
/// </summary>
public class VisDensityplot : Vis
{
    public double[,] KDEresult = null;

    public VisDensityplot()
    {
        title = "Densityplot";

        //Define Data Mark and Tick Prefab
        dataMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Sphere");
        tickMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/VisContainer/Tick");
    }

    public override GameObject CreateVis(GameObject container)
    {
        base.CreateVis(container);

        KDEresult = KernelDensityEstimation.KDE(dataSets[0].ElementAt(0).Value);
        ChangeDataMarks();

        //## 01:  Create Axes and Grids
        // X Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(0).Key, dataSets[0].ElementAt(0).Value, Direction.X);
        visContainer.CreateGrid(Direction.X, Direction.Y);

        // Y Axis
        visContainer.CreateAxis("density function", dataSets[0].ElementAt(1).Value, Direction.Y);

        //## 02: Set Remaining Vis Channels (Color,...)
        visContainer.SetChannel(VisChannel.XPos, dataSets[0].ElementAt(0).Value);
        visContainer.SetChannel(VisChannel.YPos, dataSets[0].ElementAt(1).Value);
        visContainer.SetChannel(VisChannel.Color, dataSets[0].ElementAt(1).Value);

        //## 03: Draw all Data Points with the provided Channels 
        visContainer.CreateDataMarks(dataMarkPrefab);
      
        
        //## 04: Rescale Chart
        visContainerObject.transform.localScale = new Vector3(width, height, depth);

        List<DataMark> datamarks = visContainer.dataMarkList;

        ConnectDataMarks(datamarks);
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

    /// <summary>
    /// It connects the dots with a line.
    /// </summary>
    /// <param name="datamarks"></param>
    public void ConnectDataMarks(List<DataMark>  datamarks)
    {
        //Debug.Log(dataSets[0].ElementAt(0).Value[0]);
        for (int i = 0; i < datamarks.Count-1; i++)
        {
            //
            var startTransform = datamarks[i].GetDataMarkInstance().transform;
            var endTransform = datamarks[i+1].GetDataMarkInstance().transform;
            var start = new Vector3(startTransform.localPosition.x,startTransform.localPosition.y,startTransform.localPosition.z);
            //var end = new Vector3(endTransform.localPosition.x,endTransform.localPosition.y,endTransform.localPosition.z);
            //debug(dataSets[0].ElementAt(0).Value[0])
            //Debug.DrawLine(startTransform.position, endTransform.position, Color.red);
            GameObject line = new GameObject();
            line.name = i + "";
            line.transform.localPosition = start;
            line.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            line.AddComponent<LineRenderer>();
            //line.transform.SetParent(GameObject.Find("Data Marks").transform,false);
            LineRenderer renderer = line.GetComponent<LineRenderer>();
            renderer.useWorldSpace = true;
            //(GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Sphere");
            renderer.material = (Material)Resources.Load("Prefabs/DataVisPrefabs/Marks/Line");
            renderer.material.SetColor("_Color", Color.white);
            renderer.startWidth = 0.003f;
            renderer.endWidth = 0.003f;
            renderer.SetPosition(0, startTransform.position);
            renderer.SetPosition(1, endTransform.position);

        }
    }
}
