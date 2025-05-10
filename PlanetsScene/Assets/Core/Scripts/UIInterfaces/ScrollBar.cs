using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public interface IScrollBar
{
    event UnityAction<float> onScroll;
    float NowValue { get; }
    float Size { set; }
}

public class ScrollBar : MonoBehaviour, IScrollBar
{
    [SerializeField] private Scrollbar scrollBar;

    public float NowValue => scrollBar.value;
    
    public float Size { set => scrollBar.size = Mathf.Clamp01(value); }

    public event UnityAction<float> onScroll
    {
        add => scrollBar.onValueChanged.AddListener(value);
        remove => scrollBar.onValueChanged.RemoveListener(value);
    }
}
