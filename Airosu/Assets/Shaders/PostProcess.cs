using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcess : MonoBehaviour
{

    private Material _material;
    [SerializeField]
    private Shader shader;
    
    [SerializeField]
    [Range(0, 1)]
    private float shaderIntensity;

    public float grayScale = 0;

    void Awake()
    {
        _material = new Material(shader);
    }
    
    void FixedUpdate()
    {
        _material.SetFloat("_GrayScale", grayScale);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, _material);
    }
}
