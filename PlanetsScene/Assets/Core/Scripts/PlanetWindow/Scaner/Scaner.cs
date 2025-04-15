using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Space_Screen;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Planet_Window
{
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
        [Inject] private IDrone drone;
        [Inject] private ITravelManager travelManager;
        [Inject] private IPointerManager pointer;

        private IScanerObserver observer;
        private IPointOfInterest increasingValuePOI;
        private bool isScanning;

        [SerializeField] private RectTransform colliderTransform;
        [SerializeField] private PlanetWindow_Scaner view;

        [SerializeField] private float distToDetectPlanet = 100.0f;

        [SerializeField] private RectTransform scanerArea;

        [Header("speeds")]
        [SerializeField] private float speedOfDecrease = 0.5f;
        [SerializeField] private float speedOfIncrease = 2.0f;

        private BoxCollider2D boxCollider;

        public bool IsScanning => isScanning;

        public Dictionary<IPointOfInterest, float> SeePointValueByPOI => seePointValues;

        public event Action onScanerEnable;
        public event Action onScanerDisable;
        public event Action<bool> onChangeIsScanning;
        
        private Dictionary<IPointOfInterest, float> seePointValues;
        private IPointOfInterest lastSeenPointOfInterest;
        private IPlanet scanningPlanet;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            resolver.Inject(observer = new ScanerObserver(view, this));
        }

        public void Initialize()
        {
            boxCollider = gameObject.GetComponent<BoxCollider2D>();
            seePointValues = new Dictionary<IPointOfInterest, float>();

            isScanning = false;
            increasingValuePOI = null;

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

            pointer.OnPointerUp += CheckPoint;
        }

        private void Disable()
        {
            seePointValues.Clear();

            observer.Disable();
            gameObject.SetActive(false);

            pointer.OnPointerUp -= CheckPoint;
        }

        private void CheckPoint() //TODO: высылает дрона когда мы нажимаем на кнопку выключения сканера
        {
            if(!IsMouseInArea())
            {
                return;
            }

            SetStatus(false);
            Vector2 nowWorldPos = pointer.NowWorldPosition;

            if (lastSeenPointOfInterest == null)
            {
                drone.Land(nowWorldPos, lastSeenPointOfInterest);
                return;
            }

            float dist = Vector3.Distance(pointer.NowScreenPosition, Camera.main.WorldToScreenPoint(lastSeenPointOfInterest.GameObject.transform.position));
            
            if (dist > distToDetectPlanet)
            {
                lastSeenPointOfInterest = null;
            }
            drone.Land(nowWorldPos, lastSeenPointOfInterest);

            pointer.OnPointerUp -= CheckPoint;
        }

        void OnTriggerStay2D(Collider2D other)
        {
            IPointOfInterest pointOfInterest = other.GetComponent<IPointOfInterest>();
            increasingValuePOI = pointOfInterest;
            
            if (pointOfInterest == null)
                return;
            
            IncreaseSeeValue(pointOfInterest);

            lastSeenPointOfInterest = pointOfInterest;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            increasingValuePOI = null;
        }

        private void ReduceSeeValues()
        {
            List<IPointOfInterest> listPOI = SeePointValueByPOI.Keys.ToList();
            foreach (IPointOfInterest poi in listPOI)
            {
                if (increasingValuePOI == poi)
                    continue;

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

        private bool IsMouseInArea()
        {
            if (scanerArea == null)
                return true;

            Vector2 lp;
            Vector2 mousePos = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(scanerArea, mousePos, Camera.main, out lp);

            return scanerArea.rect.Contains(lp);
        }
        public void Start() { }
    }
}