using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[DisallowMultipleComponent, ImageEffectAllowedInSceneView]
[RequireComponent(typeof(Camera))]
public class Test : MonoBehaviour
{
    public PostProcessLab.EffectLayerProfile profile;
    private void Awake()
    {
        Debug.LogError("Test awake");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.LogError("update in scene view: " + gameObject.name);
    }

    private void OnPreCull()
    {
        Debug.LogError("on precull :  " + gameObject.name);
    }
}
