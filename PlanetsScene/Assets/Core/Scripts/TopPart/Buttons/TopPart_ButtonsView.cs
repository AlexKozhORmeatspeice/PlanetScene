using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITopPart_ButtonsView
{

    void Enable();
    void Disable();
}
public class TopPart_ButtonsView : MonoBehaviour, ITopPart_ButtonsView
{
    public void Disable()
    {
        gameObject.SetActive(false);
    }
    public void Enable()
    {
        gameObject.SetActive(true);
    }
}
