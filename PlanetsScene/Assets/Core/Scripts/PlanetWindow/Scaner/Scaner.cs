using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Space_Screen;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Assertions.Must;
using VContainer;
using VContainer.Unity;

namespace Planet_Window
{
    public interface IScaner
    {
        event Action onScanerEnable;
        event Action onStartScanning;
        event Action onScanerDisable;
        event Action<bool> onChangeIsScanning;

        bool IsScanning { get; }

        Dictionary<IPointOfInterest, float> SeePointValueByPOI { get; }

        void SetStatus(bool status);
        void SwapStatus();
    }

    public class Scaner : MonoBehaviour, IScaner, IStartable, ITickable, IDisposable
    {
        [Inject] private IDrone drone;
        [Inject] private ITravelManager travelManager;
        [Inject] private IPointerManager pointer;

        private IScanerObserver observer;
        private bool isScanning;
        private bool isEnabled;

        [SerializeField] private RectTransform colliderTransform;
        [SerializeField] private PlanetWindow_Scaner view;

        [SerializeField] private float distToDetectPlanet = 100.0f;

        [Header("speeds")]
        [SerializeField] private float speedOfDecrease = 0.5f;
        [SerializeField] private float speedOfIncrease = 2.0f;

        private BoxCollider2D boxCollider;

        public bool IsScanning => isScanning;

        public Dictionary<IPointOfInterest, float> SeePointValueByPOI => seePointValues;

        public event Action onScanerEnable;
        public event Action onScanerDisable;
        public event Action<bool> onChangeIsScanning;
        public event Action onStartScanning;

        private Dictionary<IPointOfInterest, float> seePointValues;
        private IPointOfInterest lastSeenPointOfInterest;
        private IPlanet scanningPlanet;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            resolver.Inject(observer = new ScanerObserver(view, this));
        }

        public void Start()
        {
            //
        }

        public void Initialize()
        {
            boxCollider = gameObject.GetComponent<BoxCollider2D>();
            seePointValues = new Dictionary<IPointOfInterest, float>();

            isScanning = false;
            isEnabled = false;

            travelManager.onTravelToPlanet += SetScanningPlanet;

            Disable();
        }

        public void Dispose()
        {
            seePointValues.Clear();

            travelManager.onTravelToPlanet -= SetScanningPlanet;

            Disable();

            onScanerEnable = null;
            onScanerDisable = null;
        }

        public void Tick()
        {
            ReduceSeeValues();
            UpdateColliderSize();
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
            if (scanningPlanet == null)
                return;

            observer.Enable();

            lastSeenPointOfInterest = null;
            gameObject.SetActive(true);

            pointer.OnDoubleTap += StartScanning;
        }

        private void StartScanning()
        {
            pointer.OnDoubleTap -= StartScanning;

            isEnabled = true;
            onStartScanning?.Invoke();

            pointer.OnDoubleTap += CheckPoint;
        }

        private void Disable()
        {
            seePointValues.Clear();

            isEnabled = false;
            observer.Disable();
            gameObject.SetActive(false);

            pointer.OnDoubleTap -= StartScanning;
            pointer.OnDoubleTap -= CheckPoint;
        }

        private void CheckPoint()
        {
            Vector2 nowWorldPos = pointer.NowWorldPosition;

            if (lastSeenPointOfInterest == null)
            {
                drone.Land(nowWorldPos, lastSeenPointOfInterest);
                return;
            }

            float dist = Vector3.Distance(pointer.NowWorldPosition, lastSeenPointOfInterest.GameObject.transform.position);

            if (dist > distToDetectPlanet)
            {
                lastSeenPointOfInterest = null;
            }
            drone.Land(nowWorldPos, lastSeenPointOfInterest);

            SetStatus(false);
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (!isEnabled)
                return;

            IPointOfInterest pointOfInterest = other.GetComponent<IPointOfInterest>();

            if (pointOfInterest == null)
                return;

            IncreaseSeeValue(pointOfInterest);

            lastSeenPointOfInterest = pointOfInterest;
        }

        private void ReduceSeeValues()
        {
            List<IPointOfInterest> listPOI = SeePointValueByPOI.Keys.ToList();
            foreach (IPointOfInterest poi in listPOI)
            {
                SeePointValueByPOI[poi] = Mathf.Lerp(SeePointValueByPOI[poi],
                                                     0.0f,
                                                     Time.deltaTime * speedOfDecrease);
            }
        }

        private void IncreaseSeeValue(IPointOfInterest pointOfInterest)
        {
            if (!SeePointValueByPOI.ContainsKey(pointOfInterest))
            {
                SeePointValueByPOI.Add(pointOfInterest, 0.0f);
            }
            else
            {
                SeePointValueByPOI[pointOfInterest] = Mathf.Lerp(SeePointValueByPOI[pointOfInterest],
                                                             10.0f,
                                                             Time.deltaTime * speedOfIncrease);
            }
        }

        private void UpdateColliderSize()
        {
            if (boxCollider == null || colliderTransform == null)
                return;

            boxCollider.size = new Vector2(colliderTransform.sizeDelta.x, colliderTransform.sizeDelta.y);
            gameObject.transform.position = pointer.NowWorldPosition;
        }

        private void SetScanningPlanet(IPlanet planet)
        {
            scanningPlanet = planet;
        }

        private void SetSeePOIs(List<IPointOfInterest> POIs)
        {
            seePointValues.Clear();
            foreach (IPointOfInterest poi in POIs)
            {
                if (poi == null)
                    continue;
                seePointValues.Add(poi, 0.0f);
            }
        }
    }

}