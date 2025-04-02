using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VContainer.Unity;

public interface IToolTipView
{
    string Name { set; }
    string Lable { set; }
    string Description { set; }

    void Enable();
    void Disable();
    void MirrorX();
    void MirrorY();
}

public class ToolTipView : MonoBehaviour, IToolTipView
{
    [Header("Info")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text secondTitleText;
    [SerializeField] private TMP_Text descText;

    private RectTransform parentObj;
    private RectTransform selfRect;

    private Vector3 startOffset;

    private bool isMirrorX = false;
    private bool isMirrorY = false;

    public string Name { 
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
    public string Lable { 
        set
        {
            if (secondTitleText == null)
                return;

            secondTitleText.text = value;
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void FixedUpdate()
    {
        Vector3 sreenPos = Camera.main.WorldToScreenPoint(parentObj.position + startOffset);

        if (sreenPos.x + 2.0f * Mathf.Abs(selfRect.sizeDelta.x) > Screen.width)
        {
            if(!isMirrorX)
                MirrorX();
        }
        else if(isMirrorX)
        {
            MirrorX();
            isMirrorX = false;
        }

        if (sreenPos.y + 2.0f * Mathf.Abs(selfRect.sizeDelta.y) > Screen.height)
        {
            if (!isMirrorY)
                MirrorY();
        }
        else if (isMirrorY)
        {
            MirrorY();
            isMirrorY = false;
        }
    }

    public void Start()
    {
        parentObj = transform.parent.GetComponent<RectTransform>();
        selfRect = GetComponent<RectTransform>();

        startOffset = selfRect.position - parentObj.position; 
    }

    public void MirrorX()
    {
        float distX = selfRect.position.x - parentObj.position.x;
        float newX = parentObj.position.x + (-distX);

        selfRect.position = new Vector3(newX, selfRect.position.y, selfRect.position.z);
        isMirrorX = true;
    }

    public void MirrorY()
    {
        float distY = selfRect.position.y - parentObj.position.y;
        float newY = parentObj.position.y + (-distY);

        selfRect.position = new Vector3(selfRect.position.x, newY, selfRect.position.z);
        isMirrorY = true;
    }
}
