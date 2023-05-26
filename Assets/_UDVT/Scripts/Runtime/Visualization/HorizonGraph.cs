using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;

public class HorizonGraph : Vis
{
    public double[,] KDEresult = null;

    public HorizonGraph()
    {
        title = "HorizonGraph";

        //Define Data Mark and Tick Prefab
        dataMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/Marks/Sphere");
        tickMarkPrefab = (GameObject)Resources.Load("Prefabs/DataVisPrefabs/VisContainer/Tick");
    }

    // TODO: It will change
    public override GameObject CreateVis(GameObject container)
    {
        base.CreateVis(container);

        //## 01:  Create Axes and Grids

        // X Axis
        visContainer.CreateAxis(dataSets[0].ElementAt(0).Key, dataSets[0].ElementAt(0).Value, Direction.X);
        visContainer.CreateGrid(Direction.X, Direction.Y);

        // Y Axis
        visContainer.CreateAxis("frequency", dataSets[0].ElementAt(1).Value, Direction.Y);


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

}
