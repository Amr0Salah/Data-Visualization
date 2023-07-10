using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;
using static KernelDensityEstimation;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class VisViolinplot : Vis
{
    public double[,] KDEresult = null;
    private LineRenderer lineRenderer;

    public VisViolinplot()
    {
        title = "ViolinPlot";

        //Define Data Mark and Tick Prefab
        dataMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Sphere");
        //dataMarkPrefabMirror = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Sphere");
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


        double min = minimum;
        double max = maximum;

  
        ////////////////
        //## 01:  Create Axes and Grids
        // X Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(0).Key, dataSets[0].ElementAt(0).Value, Direction.X);
        visContainer.CreateGrid(Direction.X, Direction.Y);

        // Y Axis

        double[] Drawnew = new double [2];
        double[] value = dataSets[0].ElementAt(1).Value;
        for (int i = 0; i < value.Length; i++)
        {
            if (i == 0)
            {
                Drawnew[0] = value[i];
                Drawnew[1] = value[i];
            }
            if (Drawnew[0] > value[i])
            {
                Drawnew[0] = value[i];
            }
            if (Drawnew[1] < value[i])
            {
                Drawnew[1] = value[i];
            }
        }

        Drawnew[0] = (Drawnew[0]) - Drawnew[1];

        //visContainer.CreateAxis("Violin function", dataSets[0].ElementAt(1).Value, Direction.Y);
        visContainer.CreateAxis("Violin function",Drawnew, Direction.Y);

        //## 02: Set Remaining Vis Channels (Color,...)
        visContainer.SetChannel(VisChannel.XPos, dataSets[0].ElementAt(0).Value);
        visContainer.SetChannel(VisChannel.YPos, dataSets[0].ElementAt(1).Value);
        visContainer.SetChannel(VisChannel.Color, dataSets[0].ElementAt(1).Value);

        //## 03: Draw all Data Points with the provided Channels 

        visContainer.VilonCreateDataMarks(dataMarkPrefab);
        visContainer.CreateMirrorDataMarks(dataMarkPrefab);
        //## 04: Rescale Chart
        visContainerObject.transform.localScale = new Vector3(width, height, depth);
        List<DataMark> datamarks = visContainer.dataMarkList;
        ConnectDataMarks(datamarks);
        return visContainerObject;

    }

    /// <summary>
    /// Each child class can define how the Data Marks in the visualization can be changed.
    /// </summary>
    /// 
    public override void ChangeDataMarks()
    {
        var xName = dataSets[0].ElementAt(0).Key;
        var yName = dataSets[0].ElementAt(1).Key;


        dataSets[0][xName] = Enumerable.Range(0, KDEresult.GetLength(0)).Select(x => KDEresult[x, 0]).ToArray();
        dataSets[0][yName] = Enumerable.Range(0, KDEresult.GetLength(0)).Select(x => KDEresult[x, 1]).ToArray();
        

        //         dataSets[0][xName] = Enumerable.Range(0, 2 * KDEresult.GetLength(0)).Select(x => x < KDEresult.GetLength(0) ? KDEresult[x, 0] : KDEresult[2 * KDEresult.GetLength(0) - x - 1, 0]).ToArray();
        //       dataSets[0][yName] = Enumerable.Range(0, 2 * KDEresult.GetLength(0)).Select(x => x < KDEresult.GetLength(0) ? KDEresult[x, 1] : -KDEresult[2 * KDEresult.GetLength(0) - x - 1, 1]).ToArray();
        //dataSets[0][]

        /// dh awel 7aga --lama kan feh el mirror\
    }

    public void ConnectDataMarks(List<DataMark> datamarks)
    {
        for (int i = 0; i < datamarks.Count - 1; i++)
        {
            var startTransform = datamarks[i].GetDataMarkInstance().transform;
            var endTransform = datamarks[i + 1].GetDataMarkInstance().transform;
            var start = new Vector3(startTransform.localPosition.x, startTransform.localPosition.y, startTransform.localPosition.z);

            GameObject line = new GameObject();
            line.name = i + "";
            line.transform.localPosition = start;
            line.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            line.AddComponent<LineRenderer>();

            LineRenderer renderer = line.GetComponent<LineRenderer>();
            renderer.useWorldSpace = true;
            renderer.material = (Material)Resources.Load("Prefabs/DataVisPrefabs/Marks/Line");
            renderer.material.SetColor("_Color", Color.white);
            renderer.startWidth = 0.003f;
            renderer.endWidth = 0.003f;
            renderer.SetPosition(0, startTransform.position);
            renderer.SetPosition(1, endTransform.position);
            if (i == datamarks.Count / 2 - 2)
            {
                startTransform = datamarks[i].GetDataMarkInstance().transform;
                endTransform = datamarks[datamarks.Count - 1].GetDataMarkInstance().transform;
                start = new Vector3(startTransform.localPosition.x, startTransform.localPosition.y, startTransform.localPosition.z);
                line = new GameObject();
                line.name = i + 1 + "";
                line.transform.localPosition = start;
                line.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                line.AddComponent<LineRenderer>();
                renderer = line.GetComponent<LineRenderer>();
                renderer.useWorldSpace = true;
                renderer.material = (Material)Resources.Load("Prefabs/DataVisPrefabs/Marks/Line");
                renderer.material.SetColor("_Color", Color.white);
                renderer.startWidth = 0.003f;
                renderer.endWidth = 0.003f;
                renderer.SetPosition(0, startTransform.position);
                renderer.SetPosition(1, endTransform.position);

                i++;
            }
            if (i == datamarks.Count - 2)
            {
                startTransform = datamarks[0].GetDataMarkInstance().transform;
                endTransform = datamarks[datamarks.Count / 2].GetDataMarkInstance().transform;
                start = new Vector3(startTransform.localPosition.x, startTransform.localPosition.y, startTransform.localPosition.z);
                line = new GameObject();
                line.name = i + 1 + "";
                line.transform.localPosition = start;
                line.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                line.AddComponent<LineRenderer>();
                renderer = line.GetComponent<LineRenderer>();
                renderer.useWorldSpace = true;
                renderer.material = (Material)Resources.Load("Prefabs/DataVisPrefabs/Marks/Line");
                renderer.material.SetColor("_Color", Color.white);
                renderer.startWidth = 0.003f;
                renderer.endWidth = 0.003f;
                renderer.SetPosition(0, startTransform.position);
                renderer.SetPosition(1, endTransform.position);
            }


        }
    }


    public void DrawVilonShape(double minimum)
    {
        GameObject line = new GameObject();
        line.name = "AMR";
        //line.transform.localPosition = start;
        line.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        line.AddComponent<LineRenderer>();

        LineRenderer renderer = line.GetComponent<LineRenderer>();
        renderer.useWorldSpace = true;
        renderer.material = (Material)Resources.Load("Prefabs/DataVisPrefabs/Marks/Line");
        renderer.material.SetColor("_Color", Color.white);
        renderer.startWidth = 0.003f;
        renderer.endWidth = 0.003f;
        renderer.SetPosition(0, new(0.025f, 0.22f, 0f));
        renderer.SetPosition(1, new(0.045f, 0.22f, 0f));

        Debug.Log(minimum);
    }

}
