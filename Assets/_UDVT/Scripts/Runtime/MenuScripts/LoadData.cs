using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadData : MonoBehaviour
{
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
        CallMainScene();
    }

    private void CallMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}

