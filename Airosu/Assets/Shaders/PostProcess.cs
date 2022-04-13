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

    void Awake()
    {
        _material = new Material(shader);
    }
    
    void FixedUpdate()
    {
        _material.SetFloat("_GrayscaleAmount", shaderIntensity);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, _material);
    }

    public IEnumerator SetGrayscale(float newValue, float duration)
    {
        float time = 0;
        bool isReducing = newValue < shaderIntensity;
        while (duration > time)
        {
            float durationFrame = Time.deltaTime;
            float ratio = time / duration;
            shaderIntensity = isReducing ? 1 - ratio : ratio;
            time += durationFrame;
            yield return null;
        }
        shaderIntensity = newValue;
    }
}
