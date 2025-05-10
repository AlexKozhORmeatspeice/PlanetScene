using System;
using Cysharp.Threading.Tasks.Triggers;
using Frontier_anim;
using Game_UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Settings_Screen
{
    public interface ISettingsSlotSlider : ISettingsSlot
    { 
        event UnityAction<float> OnSliderValueChanged;

        string Text { set; }
        float SliderValue { get; }
       
        public void AnimateChoosed();
        public void AnimateUnchoosed();
    }

    public class SettingsSlotSlider : MonoBehaviour, ISettingsSlotSlider
    {
        [Header("BG colors")]
        [SerializeField] private Color baseBgColor;
        [SerializeField] private Color choosedBgColor;

        [Header("Slider colors")]
        [SerializeField] private Color baseSliderAreaColor;
        [SerializeField] private Color choosedSliderAreaColor;

        [Header("Text color")]
        [SerializeField] private Color baseTxtColor;
        [SerializeField] private Color choosedTxtColor;

        [Header("Objs")]
        [SerializeField] private Image slotBgImg;
        [SerializeField] private TMP_Text labelTxt;
        
        [SerializeField] private TMP_Text sliderTxt;
        [SerializeField] private Image sliderBgImg;
        [SerializeField] private Image sliderAreaImg;
        [SerializeField] private Image sliderHandleImg;

        [SerializeField] private GameSlider sld;

        public string Text { set => sld.Text = value; }

        public float SliderValue => sld.Value;

        public event UnityAction<float> OnSliderValueChanged
        {
            add => sld.onChangeValue += value;
            remove => sld.onChangeValue -= value;
        }

        public void AnimateChoosed()
        {
            AnimService.Instance.PlayAnim<ChooseSlot>(gameObject);
        }

        public void AnimateUnchoosed()
        {
            AnimService.Instance.PlayAnim<UnchooseSlot>(gameObject);
        }

        private void Awake()
        {
            AnimService.Instance.BuildAnim<ChooseSlot>(gameObject)
                .AddImage(slotBgImg, choosedBgColor)
                .AddImage(sliderAreaImg, choosedSliderAreaColor)
                .AddImage(sliderHandleImg, choosedSliderAreaColor)
                .AddText(labelTxt, choosedTxtColor)
                .AddText(sliderTxt, choosedTxtColor)
                .Build();

            AnimService.Instance.BuildAnim<UnchooseSlot>(gameObject)
                .AddImage(slotBgImg, baseBgColor)
                .AddImage(sliderAreaImg, baseSliderAreaColor)
                .AddImage(sliderHandleImg, baseSliderAreaColor)
                .AddText(labelTxt, baseTxtColor)
                .AddText(sliderTxt, baseTxtColor)
                .Build();
        }
    }
}