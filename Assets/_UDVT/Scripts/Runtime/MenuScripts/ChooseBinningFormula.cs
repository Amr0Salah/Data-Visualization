using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// It contains formulas used in calculating the binning.
/// </summary>
public class ChooseBinningFormula : MonoBehaviour
{
    //NewCode_12
    public void StartWithSquareroot()
    {
        CurrentParams.currentBinningType = BinningType.Squareroot;
        SceneManager.LoadScene("LoadData");
    }

    public void StartWithSturges()
    {
        CurrentParams.currentBinningType = BinningType.Sturges;
        SceneManager.LoadScene("LoadData");
    }
}
