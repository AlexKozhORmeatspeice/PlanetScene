using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public interface IGraphicView
{
    delegate float Func(float x);

    void Render();
    void SetFunction(Func func);

    void Enable();
    void Disable();
}

public class GraphicView : MonoBehaviour, IGraphicView
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int linesCountForOneX;
    
    [Header("Ranges")]
    [SerializeField] private Vector2 xrange;
    [SerializeField] private Vector2 yrange;
    
    [Header("Points")]
    [SerializeField] private Transform anchorPoint;
    [SerializeField] private Transform yPoint;
    [SerializeField] private Transform xPoint;

    private float width;
    private float height;

    IGraphicView.Func nowFunc;

    public void Enable()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = (int)xrange.y * linesCountForOneX + 1;

        width = Vector2.Distance(anchorPoint.position, xPoint.position);
        height = Vector2.Distance(anchorPoint.position, yPoint.position);
    }

    public void Disable()
    {
        lineRenderer.enabled = false;
    }

    public void Render()
    {
        if(nowFunc == null)
        {
            Debug.LogError("No function assigned in Graphic");
            return;
        }

        float posX = xrange.x;
        float posY = nowFunc(posX);

        float step = 1.0f / (float)linesCountForOneX;

        for(int i = 0; i < lineRenderer.positionCount; i++)
        {
            float normPosX = Mathf.Clamp01((posX - xrange.x) / (xrange.y - xrange.x));
            float normPosY = Mathf.Clamp01((posY - yrange.x) / (yrange.y - yrange.x));

            Vector3 newPos = Vector3.zero;
            
            newPos.x = anchorPoint.position.x * (1.0f - normPosX) + xPoint.position.x * normPosX;

            newPos.y = anchorPoint.position.y * (1.0f - normPosY) + yPoint.position.y * normPosY;
            newPos.z = -1.0f;

            lineRenderer.SetPosition(i, newPos);

            posX += step;
            posY = nowFunc(posX);
        }
    }

    public void SetFunction(IGraphicView.Func func)
    {
        nowFunc = func;
        Render();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
