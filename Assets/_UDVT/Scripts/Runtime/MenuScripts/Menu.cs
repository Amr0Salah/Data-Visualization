using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartWithScatterplot()
    {
        CurrentVisType.currentVisType = VisType.Scatterplot;
        CallMainScene();
    }

    public void StartWithHistogram()
    {
        CurrentVisType.currentVisType = VisType.Histogram;
        CallMainScene();
    }

    public void StartWithDensityplot()
    {
        CurrentVisType.currentVisType = VisType.Densityplot;
        CallMainScene();
    }

    public void StartWithViolinPlot()
    {
        CurrentVisType.currentVisType = VisType.ViolinPlot;
        CallMainScene();
    }

    public void StartWithHorizonGraph()
    {
        CurrentVisType.currentVisType = VisType.HorizonGraph;
        CallMainScene();
    }

    private void CallMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
