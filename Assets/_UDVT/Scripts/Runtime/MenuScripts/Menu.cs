using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartWithScatterplot()
    {
        CurrentParams.currentVisType = VisType.Scatterplot;
        CallLoadData();
    }

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

    private void CallLoadData()
    {
        SceneManager.LoadScene("LoadData");
    }
}
