using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// It contains the variables defined to move the parameters 
/// taken with the help of the menu between scenes.
/// </summary>
public class CurrentParams : MonoBehaviour
{
    public static VisType currentVisType = VisType.Scatterplot; // default
    public static BinningType currentBinningType = BinningType.Squareroot; // default
    public static Dictionary<string, double[]> loadedData = null; // default
}
