using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPLanetWindow_GraphicWindow
{
    void Enable();
    void Disable();
}

public class PLanetWindow_GraphicWindow : MonoBehaviour, IPLanetWindow_GraphicWindow
{
    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
