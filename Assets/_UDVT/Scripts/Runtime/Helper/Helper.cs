using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// It contains the common methods used in the project.
/// </summary>
public class Helper : MonoBehaviour
{
	public static bool FileValidation()
	{
        bool result = true;
        var _len = CurrentParams.loadedData.Count;

        switch (CurrentParams.currentVisType)
        {
            case VisType.Histogram:
            case VisType.Densityplot:

                if (_len <= 0)
                    result = false;
                else if(_len == 1)
                {
                    var _key = CurrentParams.loadedData.Take(1).Select(d => d.Key).First();
                    CurrentParams.loadedData.Add("y_axis", CurrentParams.loadedData[_key]);
                }

                break;

            case VisType.Scatterplot:
            default:
                if (_len <= 2)
                    result = false;

                break;
        }


        return result;
	}
}