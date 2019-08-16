using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class TestUtil
{
    [MenuItem("Test/AddComponentToSceneView")]
    public static void AddCommonToSceneView()
    {

        var cams = SceneView.GetAllSceneCameras();
        foreach(var cam in cams)
        {
            cam.gameObject.AddComponent<Test>();
            var test = cam.gameObject.GetComponent<Test>();
            if(test == null)
            {
                Debug.LogError("can not add Component to scnen camera");
            }
        }
    }
}
