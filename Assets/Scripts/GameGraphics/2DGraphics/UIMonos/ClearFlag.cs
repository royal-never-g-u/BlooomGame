using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearFlag : MonoBehaviour
{
    RenderTexture renderTexture;
    // Start is called before the first frame update
    void Start()
    {
        renderTexture = gameObject.GetComponent<Camera>().targetTexture;   
    }

    private void OnPreRender()
    {
        renderTexture.Release();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
