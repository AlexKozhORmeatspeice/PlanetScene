using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public interface IToolTip
{
    void Show();
    void Hide();

    string Name { set; }
    string Lable { set; }
    string Description { set; }

    Vector3 Position { get; set; }
    Vector3 IconPos { get; set; }
    Vector2 WorldSize { get; }
    Vector2 ScreenSize { get; }
    Vector3 StartOffset { get; }
}

public class ToolTip : MonoBehaviour, IToolTip, IStartable
{
    [Header("Anim settings")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float playTime = 0.1f;

    [Header("Info")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text secondTitleText;
    [SerializeField] private TMP_Text descText;

    [SerializeField] private RectTransform iconRect;
    private RectTransform selfRect;
    private Vector3 startOffset;

    public string Name
    {
        set
        {
            if (titleText == null)
                return;

            titleText.text = value;
        }
    }

    public string Description
    {
        set
        {
            if (descText == null)
                return;

            descText.text = value;
        }
    }
    public string Lable
    {
        set
        {
            if (secondTitleText == null)
                return;

            secondTitleText.text = value;
        }
    }

    public Vector2 WorldSize
    {
        get
        {
            float distX = Mathf.Abs(iconRect.lossyScale.x);
            float distY = Mathf.Abs(iconRect.lossyScale.y);
            
            return new Vector2(distX, distY);
        }
    }


    public Vector2 ScreenSize
    {
        get
        {
            float distX = 2.0f * Mathf.Abs(iconRect.sizeDelta.x);
            float distY = 2.0f * Mathf.Abs(iconRect.sizeDelta.y);

            return new Vector2(distX, distY);
        }
    }

    public Vector3 Position 
    {
        get => gameObject.transform.position;
        set => gameObject.transform.position = value;
    }

    public Vector3 IconPos
    {
        get => iconRect.position;
        set => iconRect.position = value;
    }

    public Vector3 StartOffset => startOffset;

    public void Show()
    {
        canvasGroup.DOFade(1.0f, playTime);
    }

    public void Hide()
    {
        canvasGroup.DOFade(0.0f, playTime);
    }

    public void Initialize()
    {
        selfRect = GetComponent<RectTransform>();
        startOffset = iconRect.position - selfRect.position;
    }
    public void Start() { }
}
