using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpaceWindow_Map
{
    Vector3 Position { get; set; }
}

public class SpaceWindow_Map : MonoBehaviour, ISpaceWindow_Map
{
    [SerializeField] private RectTransform rectTransform;

    public Vector3 Position 
    { 
        get 
        {
            return rectTransform.localPosition;
        }
        set
        {
            rectTransform.localPosition = value;
        }
    }
}
