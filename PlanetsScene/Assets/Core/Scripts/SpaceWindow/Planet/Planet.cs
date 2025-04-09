using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Planet_Window;

namespace Space_Screen
{
    public interface IPlanet
    {
        void ChangeScale(float t01);
        void ChangeScale(bool isMaxScale);
        List<PointInfo> PointInfos { get; }
        Vector3 Position { get; }
        Vector3 Size { get; }

        string Name { get; set; }
        string SectornName { get; set; }
        string Description { get; set; }
    }

    public class Planet : MonoBehaviour, IPlanet, IScreenObject
    {
        [Header("Planet rect")]
        [SerializeField] private RectTransform rectTransformPlanet;
        [SerializeField] private Vector2 closeSize;
        [SerializeField] private Vector2 openSize;

        [Header("Button")]
        [SerializeField] private Image buttonBg;
        [SerializeField] private TMP_Text buttonText;

        [Header("Name")]
        [SerializeField] private RawImage nameBg;
        [SerializeField] private TMP_Text txtName;

        [Header("Background")]
        [SerializeField] private RawImage backgroundImage;

        [Header("Info")]
        [SerializeField] private string planetName;
        [SerializeField] private string sector;
        [SerializeField] private string description;

        [Header("Points of interest")]
        [SerializeField] private List<PointInfo> points;

        public string Name
        {
            get => txtName.text;
            set => txtName.text = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public string SectornName
        {
            get => sector;
            set => sector = value;
        }

        public Vector3 Position => gameObject.transform.position;

        public Vector3 Size => gameObject.transform.localScale;

        List<PointInfo> IPlanet.PointInfos => points;

        public void ChangeScale(float t01)
        {
            if (rectTransformPlanet == null)
                return;

            t01 = Mathf.Clamp01(t01);

            //размер
            rectTransformPlanet.sizeDelta = openSize * t01 + closeSize * (1.0f - t01);

            //прозрачность фона имени
            Color nameBgColor = nameBg.color;
            nameBgColor.a = t01;
            nameBg.color = nameBgColor;

            //прозрачность кнопки
            Color btnColor = buttonBg.color;
            btnColor.a = t01;
            buttonBg.color = btnColor;

            Color btnTextColor = buttonText.color;
            btnTextColor.a = t01;
            buttonText.color = btnTextColor;

            //прозрачность фона
            Color bgColor = backgroundImage.color;
            bgColor.a = t01;
            backgroundImage.color = bgColor;
        }

        public void ChangeScale(bool isMaxScale)
        {
            ChangeScale(isMaxScale ? 1.0f : 0.0f);
        }

        private void Awake()
        {
            Name = planetName;
            SectornName = sector;
            Description = description;
        }
    }
}