using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class SpaceWindow_PlanetView : MonoBehaviour, IGradientChangable
{
    [Header("Planet rect")]
    [SerializeField] private RectTransform rectTransformPlanet;
    [SerializeField] private Vector2 closeSize;
    [SerializeField] private Vector2 openSize;

    [Header("Button")]
    [SerializeField] private Image buttonBg;
    [SerializeField] private TMP_Text buttonText;

    [Header("Name")]
    [SerializeField] private RawImage nameBg;
    [SerializeField] private TMP_Text txtName;

    [Header("Background")]
    [SerializeField] private RawImage backgroundImage;
    
    public string Name { set => txtName.text = value; }

    public void SetVisibility(float alpha)
    {
        if (rectTransformPlanet == null)
            return;

        alpha = Mathf.Clamp01(alpha);
        
        //размер
        rectTransformPlanet.sizeDelta = openSize * alpha + closeSize * (1.0f - alpha);

        //прозрачность фона имени
        Color nameBgColor = nameBg.color;
        nameBgColor.a = alpha;
        nameBg.color = nameBgColor;

        //прозрачность кнопки
        Color btnColor = buttonBg.color;
        btnColor.a = alpha;
        buttonBg.color = btnColor;

        Color btnTextColor = buttonText.color;
        btnTextColor.a = alpha;
        buttonText.color = btnTextColor;

        //прозрачность фона
        Color bgColor = backgroundImage.color;
        bgColor.a = alpha;
        backgroundImage.color = bgColor;
    }

    public void SetVisibility(bool isVisible)
    {
        SetVisibility(isVisible ? 1.0f : 0.0f);
    }
}
