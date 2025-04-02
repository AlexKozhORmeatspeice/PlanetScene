using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IPlanetWindow_Info
{
    string Name { set; }
    string Title { set; }

    string Description { set; }

    void HideInfo();
    void ShowInfo();
}


public class PlanetWindow_Info : MonoBehaviour, IPlanetWindow_Info
{
    [Header("Info text")]
    [SerializeField] private TMP_Text sectorTxt;
    [SerializeField] private TMP_Text nameTxt;
    [SerializeField] private TMP_Text descriptionTxt;

    private string lastSectorTxt = "";
    private string lastNameTxt = "";
    private string lastDescTxt = "";
    
    string IPlanetWindow_Info.Title { 
        set 
        {
            lastSectorTxt = value;
            sectorTxt.text = value;
        }
    }
    string IPlanetWindow_Info.Name { 
        set
        {
            lastNameTxt = value;
            nameTxt.text = value;
        } 
    }
    string IPlanetWindow_Info.Description { 
        set
        {
            lastDescTxt = value;
            descriptionTxt.text = value;
        }
    }

    public void HideInfo()
    {
        lastSectorTxt = sectorTxt.text;
        lastNameTxt = nameTxt.text;
        lastDescTxt = descriptionTxt.text;

        sectorTxt.text = "";
        nameTxt.text = "";
        descriptionTxt.text = "";

        gameObject.SetActive(false);
    }

    public void ShowInfo()
    {
        sectorTxt.text = lastSectorTxt;
        nameTxt.text = lastNameTxt;
        descriptionTxt.text = lastDescTxt;

        gameObject.SetActive(true);
    }
}
