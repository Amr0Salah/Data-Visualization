using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It contains the scene where the graph will be plotted.
/// </summary>
public class MainScript : MonoBehaviour
{
    //NewCode_12
    private Vis vis;

    // Start is called at the beginning of the application
    async void Start()
    {
        Visualize();
    }

    void Update()
    {
        // If vis is not null, update the grids
        if (vis != null)
        {
            vis.UpdateGrids();
        }
        
    }

    public async void Visualize()
    {
        //NewCode_12
        //## 03: Visualize Dataset
        vis = Vis.GetSpecificVisType(CurrentParams.currentVisType);
        vis.AppendData(CurrentParams.loadedData);
        vis.CreateVis(this.gameObject);
    }
}
