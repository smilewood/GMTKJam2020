using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapCombiner : MonoBehaviour
{
    public RenderTexture[] textures;
    public RenderTexture target;
    public Material combineMat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(RenderTexture tex in textures)
        {
            Graphics.Blit(tex, target, combineMat);
        }
    }
}
