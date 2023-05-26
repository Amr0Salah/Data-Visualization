using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentParams : MonoBehaviour
{
    public static VisType currentVisType = VisType.Scatterplot; // default
    public static Dictionary<string, double[]> loadedData = null; // default
}
