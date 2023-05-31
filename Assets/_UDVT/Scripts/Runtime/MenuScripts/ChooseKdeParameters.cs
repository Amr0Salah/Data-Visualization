using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseKdeParameters : MonoBehaviour
{
    public void OnChangeSigmaValue(float val)
    {
        CurrentParams.kdeSigmaValue = (double) val;
    }

    public void OnChangeStepsValue(int val)
    {
        CurrentParams.kdeStepsValue = val;
    }

    public void SaveValuesAndRedirect()
    {
        SceneManager.LoadScene("LoadData");
    }
}
