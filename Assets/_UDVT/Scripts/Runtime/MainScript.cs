using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MainScript handles the activities needed at the start of the application.
/// </summary>
public class MainScript : MonoBehaviour
{
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
        //## 03: Visualize Dataset
        vis = Vis.GetSpecificVisType(CurrentParams.currentVisType);
        vis.AppendData(CurrentParams.loadedData);
        vis.CreateVis(this.gameObject);
    }
}
