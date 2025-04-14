using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public interface ITopPart_GoBack
{
    event UnityAction onClick;
}

public class TopPart_GoBack : MonoBehaviour, ITopPart_GoBack
{
    [SerializeField] private Button button;

    public event UnityAction onClick
    {
        add => button.onClick.AddListener(value);
        remove => button.onClick.RemoveListener(value);
    }
}
