using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Planet_Window;
using Game_UI;
using DG.Tweening;
using Load_screen;
using Frontier_anim;

namespace Space_Screen
{
    public interface IPlanet
    {
        List<PointInfo> PointInfos { get; }
        Vector3 Position { get; }
        Vector3 Size { get; }

        IIconButton encBtn { get; }

        string Name { get; set; }
        string SectornName { get; set; }
        string Description { get; set; }

        void AnimateHover();
        void AnimateChoose();
        void AnimateBase();
    }

    public class Planet : MonoBehaviour, IPlanet, IScreenObject
    {
        [SerializeField] private IconButton encyclopediaButton;

        [Header("Anim settings")]
        [SerializeField] private float playTime = 0.1f;

        [Header("Lines")]
        [SerializeField] private CanvasGroup linesCanvasGroup;

        [Header("Planet rect")]
        [SerializeField] private RectTransform rectTransformPlanet;
        [SerializeField] private Vector2 closeSize;
        [SerializeField] private Vector2 openSize;

        [Header("Button")]
        [SerializeField] private Image buttonIcon;
        
        [Header("Name")]
        [SerializeField] private Image nameOutline;
        [SerializeField] private Image nameBg;
        [SerializeField] private TMP_Text txtName;

        [Header("Background")]
        [SerializeField] private Image glowImg;
        [SerializeField] private PulsObj pulsBG;
        [SerializeField] private RotatingObj rotatingBG;
        [SerializeField] private Image backgroundImage;

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

        public IIconButton encBtn => encyclopediaButton;

        private bool isChoosed;
        private bool isBase;
        private bool isHover;

        public void AnimateBase()
        {
            if (isBase) return;

            rotatingBG.StopAnim();
            pulsBG.StopAnim();

            AnimService.Instance.PlayAnim<SetBasePlanet>(gameObject);

            isChoosed = false;
            isBase = true;
            isHover = false;
        }

        public void AnimateChoose()
        {
            if(isChoosed) return;

            rotatingBG.StartAnim();

            AnimService.Instance.PlayAnim<ChoosePlanet>(gameObject);

            isChoosed = true;
            isBase = false;
            isHover = false;
        }

        public void AnimateHover()
        {
            if (isHover) return;

            rotatingBG.StopAnim();
            pulsBG.StopAnim();

            AnimService.Instance.PlayAnim<HoverPlanet>(gameObject);

            isChoosed = false;
            isBase = false;
            isHover = true;
        }

        private void AnimateFadeInBg()
        {
            backgroundImage.DOFade(pulsBG.GetNowColor().a, playTime)
                .OnComplete(() =>
                {
                    pulsBG.StartAnim();
                });
        }

        private void Awake()
        {
            Name = planetName;
            SectornName = sector;
            Description = description;

            isChoosed = false;

            AnimService.Instance.BuildAnim<ChoosePlanet>(gameObject)
                .SetRect(rectTransformPlanet, openSize, closeSize)
                .SetNameData(nameOutline, nameBg, txtName)
                .SetBtnBg(buttonIcon)
                .SetLinesCanvasGroup(linesCanvasGroup)
                .SetBackgroundImage(backgroundImage)
                .SetGlowBg(glowImg)
                .SetCallback(AnimateFadeInBg)
                .Build();

            AnimService.Instance.BuildAnim<HoverPlanet>(gameObject)
                .SetRect(rectTransformPlanet, openSize, closeSize)
                .SetNameData(nameOutline, nameBg, txtName)
                .SetBtnBg(buttonIcon)
                .SetLinesCanvasGroup(linesCanvasGroup)
                .SetBackgroundImage(backgroundImage)
                .SetGlowBg(glowImg)
                .Build();

            AnimService.Instance.BuildAnim<SetBasePlanet>(gameObject)
                .SetRect(rectTransformPlanet, openSize, closeSize)
                .SetNameData(nameOutline, nameBg, txtName)
                .SetBtnBg(buttonIcon)
                .SetLinesCanvasGroup(linesCanvasGroup)
                .SetBackgroundImage(backgroundImage)
                .SetGlowBg(glowImg)
                .Build();
        }
    }
}