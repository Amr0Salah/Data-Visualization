using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

/// <summary>
/// It includes input buttons to obtaining the parameters required in KDE calculations from the user.
/// </summary>
public class ChooseKdeParameters : MonoBehaviour
{
    public InputField sigmaValue;
    public InputField stepsValue;

    public void OnChangeSigmaValue()
    {
        string inputText = (string) sigmaValue.text;

        if (double.TryParse(inputText, out double doubleValue))
            CurrentParams.kdeSigmaValue = doubleValue;
        else
            CurrentParams.SetDefaultKdeSigmaValue(); // set default
    }

    public void OnChangeStepsValue()
    {
        string inputText = (string)stepsValue.text;

        if (Int32.TryParse(inputText, out int intValue))
            CurrentParams.kdeStepsValue = intValue;
        else
            CurrentParams.SetDefaultKdeStepsValue(); // set default
    }

    public void SaveValuesAndRedirect()
    {
        SceneManager.LoadScene("LoadData");
    }
}
