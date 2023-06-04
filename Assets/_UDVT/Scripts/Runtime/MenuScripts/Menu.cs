using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Menu handles the activities needed at the start of the application.
/// </summary>
public class Menu : MonoBehaviour
{
    public void StartWithScatterplot()
    {
        CurrentParams.currentVisType = VisType.Scatterplot;
        CallLoadData();
    }

    /// <summary>
    /// Some charts need extra information. Therefore, before going to the LoadData screen,
    /// it goes to an intermediate screen.
    /// </summary>
    public void StartWithHistogram()
    {
        CurrentParams.currentVisType = VisType.Histogram;
        SceneManager.LoadScene("ChooseBinningFormula");
    }

    public void StartWithDensityplot()
    {
        CurrentParams.currentVisType = VisType.Densityplot;
        CallLoadData();
    }

    public void StartWithViolinPlot()
    {
        CurrentParams.currentVisType = VisType.ViolinPlot;
        CallLoadData();
    }

    public void StartWithHorizonGraph()
    {
        CurrentParams.currentVisType = VisType.HorizonGraph;
        CallLoadData();
    }

    /// <summary>
    /// It directs to the LoadData to load the data needed to draw the selected graph.
    /// </summary>
    private void CallLoadData()
    {
        SceneManager.LoadScene("LoadData");
    }
}
