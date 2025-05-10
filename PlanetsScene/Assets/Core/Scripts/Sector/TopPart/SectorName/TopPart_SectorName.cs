using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface ITopPart_SectorName
{
    string Name { set; }

    void Enable();
    void Disable();
}

public class TopPart_SectorName : MonoBehaviour, ITopPart_SectorName
{
    [SerializeField] private TMP_Text sectorName;

    public string Name { set => sectorName.text = value; }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }
}
