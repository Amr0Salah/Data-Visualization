using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseBinningFormula : MonoBehaviour
{
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
