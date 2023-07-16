using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// LoadData, the field to load the data required for ploting the selected graph.
/// </summary>
public class LoadData : MonoBehaviour
{
    //NewCode_12
    private FileLoadingManager fileLoadingManager;

    void Awake()
    {
        fileLoadingManager = new FileLoadingManager();
    }

    public void Load()
    {
        InnerLoad();
    }

    private async void InnerLoad()
    {
        //## 01: Load Dataset

        string filePath = fileLoadingManager.StartPicker();

        // Application waits for the loading process to finish
        FileType file = await fileLoadingManager.LoadDataset();

        if (file == null) return; //Nothing loaded

        //## 02: Process Dataset
        CsvFileType csvFile = (CsvFileType)file;
        CurrentParams.loadedData = csvFile.GetDataSet();
        
        //We are checking whether the loaded data is suitable for the graph.
        if (Helper.FileValidation())
        {
            if(CurrentParams.currentVisType == VisType.Densityplot)
                SceneManager.LoadScene("ChooseKdeParameters");
            else
                CallMainScene();
        }
        else
            Debug.LogError(CurrentParams.currentVisType + " cannot be plot with the csv file you uploaded.");


    }

    /// <summary>
    /// It directs to the screen where the graphic will be drawn.
    /// </summary>
    private void CallMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}

