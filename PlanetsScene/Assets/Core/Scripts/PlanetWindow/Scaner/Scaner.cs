using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Assertions.Must;
using VContainer;
using VContainer.Unity;

public interface IScaner
{
    event Action onScanerEnable;
    event Action onScanerDisable;
    event Action<bool> onChangeIsScanning;

    bool IsScanning { get; }

    Dictionary<IPointOfInterest, float> SeePointValueByPOI { get; }

    void SetStatus(bool status);
    void SwapStatus();
}

public class Scaner : MonoBehaviour, IScaner, IStartable, ITickable, IDisposable
{
    [Inject] private INotFound notFound;
    [Inject] private IDrone drone;
    [Inject] private IPointerManager pointer;
    
    private IPlanetWindow_ScanerObserver observer;
    private bool isScanning;
    private bool isEnabled;

    [SerializeField] private RectTransform colliderTransform;
    [SerializeField] private PlanetWindow_Scaner view;

    [SerializeField] private float distToDetectPlanet = 100.0f;

    [Header("speeds")]
    [SerializeField] private float speedOfDecrease = 0.5f;
    [SerializeField] private float speedOfIncrease = 2.0f;
    [Header("Objs")]
    [SerializeField] private GameObject filedObj;

    private BoxCollider2D boxCollider;

    public bool IsScanning => isScanning;

    public Dictionary<IPointOfInterest, float> SeePointValueByPOI => seePointValues;

    public event Action onScanerEnable;
    public event Action onScanerDisable;
    public event Action<bool> onChangeIsScanning;

    private Dictionary<IPointOfInterest, float> seePointValues;
    private IPointOfInterest lastSeenPointOfInterest;

    [Inject]
    public void Construct(IObjectResolver resolver)
    {
        resolver.Inject(observer = new PlanetWindow_ScanerObserver(view));
    }

    public void Start()
    {
        //
    }

    public void Initialize()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        isScanning = false;
        seePointValues = new Dictionary<IPointOfInterest, float>();

        Disable();
    }

    public void Dispose()
    {
        Disable();
        onScanerEnable = null;
        onScanerDisable = null;
        seePointValues.Clear();
    }

    public void Tick() //update collider
    {
        if (this == null || colliderTransform == null)
            return;

        ReduceSeeValues();

        boxCollider.size = new Vector2(colliderTransform.sizeDelta.x, colliderTransform.sizeDelta.y);
        gameObject.transform.position = pointer.NowWorldPosition;
    }

    public void SetStatus(bool status)
    {
        if (isScanning != status)
        {
            onChangeIsScanning?.Invoke(status);
        }

        isScanning = status;

        if (isScanning)
        {
            Enable();
            onScanerEnable?.Invoke();
        }
        else
        {
            Disable();
            onScanerDisable?.Invoke();
        }

        isEnabled = false;
    }

    public void SwapStatus()
    {
        SetStatus(!isScanning);
    }

    private void Enable()
    {
        lastSeenPointOfInterest = null;
        observer.Enable();
        gameObject.SetActive(true);
        SetSeeValues();

        pointer.OnDoubleTap += WaitForFixedPos;

    }

    private void WaitForFixedPos()
    {
        pointer.OnDoubleTap -= WaitForFixedPos;

        isEnabled = true;
        filedObj.SetActive(true);

        pointer.OnDoubleTap += CheckPoint;
    }

    private void Disable()
    {
        isEnabled = false;
        observer.Disable();
        gameObject.SetActive(false);
        filedObj.SetActive(false);

        pointer.OnDoubleTap -= WaitForFixedPos;
        pointer.OnDoubleTap -= CheckPoint;
    }

    private void CheckPoint()
    {
        //////functions for land/////
        Vector2 nowWorldPos = pointer.NowWorldPosition;
        void SetPOIActive() {
            lastSeenPointOfInterest.SetVisibility(true);
            lastSeenPointOfInterest.SetInterective(true);
            drone.onLand -= SetPOIActive;
        }
        void SetNotFoundActive()
        {
            notFound.Enable(nowWorldPos);
            drone.onLand -= SetNotFoundActive;
        }
        /////////////////////////////
        
        SetStatus(false);
        drone.Land(pointer.NowWorldPosition);

        if (lastSeenPointOfInterest == null)
        {
            drone.onLand += SetNotFoundActive;
            return;
        }
        
        float dist = Vector3.Distance(pointer.NowWorldPosition, lastSeenPointOfInterest.GameObject.transform.position);

        if(dist <= distToDetectPlanet)
        {
            drone.onLand += SetPOIActive;
        }
        else
        {
            drone.onLand += SetNotFoundActive;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!isEnabled)
            return;

        IPointOfInterest pointOfInterest = other.GetComponent<IPointOfInterest>();
        
        if (pointOfInterest == null)
            return;
        
        if(!SeePointValueByPOI.ContainsKey(pointOfInterest))
        {
            SeePointValueByPOI.Add(pointOfInterest, 0.0f);
        }
        else
        {
            SeePointValueByPOI[pointOfInterest] = Mathf.Lerp(SeePointValueByPOI[pointOfInterest],
                                                         10.0f,
                                                         Time.deltaTime * speedOfIncrease);
        }
        
        lastSeenPointOfInterest = pointOfInterest;
    }

    void SetSeeValues()
    {
        SeePointValueByPOI.Clear();
        
    }

    void ReduceSeeValues()
    {
        List<IPointOfInterest> listPOI = SeePointValueByPOI.Keys.ToList();
        foreach (IPointOfInterest poi in listPOI)
        {
            SeePointValueByPOI[poi] = Mathf.Lerp(SeePointValueByPOI[poi], 
                                                 0.0f, 
                                                 Time.deltaTime * speedOfDecrease);
        }
    }
}
