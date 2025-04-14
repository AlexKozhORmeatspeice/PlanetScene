using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace Planet_Window
{

}
public interface IPlanetWindow_Scaner
{
    bool isVisible { set; }
    bool scanerFiledIsVisible { set; }
    Texture Texture { set; }
    void ChangeSize(float t); // t == 0 - minSize, t == 1 - maxSize
    float SpeedOfChange { get; }
    float MouseSpeedInfluence { get; }
}

public class PlanetWindow_Scaner : MonoBehaviour, IPlanetWindow_Scaner, IDisposable
{
    [Header("Settings")]
    [SerializeField] private RawImage image;
    [SerializeField] private Vector2 maxSize;
    [SerializeField] private Vector2 minSize;
    [Range(0.01f, 3.0f)][SerializeField] private float speedOfChange = 1.0f;
    [Range(0.01f,1.0f)] [SerializeField] private float mouseSpeedInfluence = 0.01f;

    [Header("Objs")]
    [SerializeField] private RectTransform rectTransform;

    public float SpeedOfChange => speedOfChange;

    public float MouseSpeedInfluence => mouseSpeedInfluence;

    public Texture Texture { set => image.texture = value; }
    public bool isVisible { set => gameObject.SetActive(value); }
    public bool scanerFiledIsVisible { set => image.gameObject.SetActive(value); }

    public void ChangeSize(float t)
    {
        if (t == float.NaN)
            return;

        rectTransform.sizeDelta = Vector2.Lerp(minSize, maxSize, t);
    }

    public void Dispose()
    {
        rectTransform.sizeDelta = minSize;
    }
}
