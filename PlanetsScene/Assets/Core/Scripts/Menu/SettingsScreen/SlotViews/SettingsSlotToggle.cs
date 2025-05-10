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
    public interface ISettingsSlotToggle : ISettingsSlot
    {
        event UnityAction<bool> OnToggleChangeValue;


        public void AnimateChoosed();
        public void AnimateUnchoosed();
    }

    public class SettingsSlotToggle : MonoBehaviour, ISettingsSlotToggle
    {
        [Header("BG colors")]
        [SerializeField] private Color baseBgColor;
        [SerializeField] private Color choosedBgColor;

        [Header("Toggle colors")]
        [SerializeField] private Color toggleBaseColor;
        [SerializeField] private Color toggleChoosedColor;

        [Header("Text color")]
        [SerializeField] private Color baseTxtColor;
        [SerializeField] private Color choosedTxtColor;

        [Header("Objs")]
        [SerializeField] private Image slotBgImg;
        [SerializeField] private TMP_Text labelTxt;

        [SerializeField] private Image toggleBgImg;
        [SerializeField] private Image toggleCheckmarkImg;

        [SerializeField] private GameToggle toggle;

        public event UnityAction<bool> OnToggleChangeValue
        {
            add => toggle.onValueChanged += value;
            remove => toggle.onValueChanged -= value;
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
                .AddImage(toggleBgImg, toggleChoosedColor)
                .AddImage(toggleCheckmarkImg, toggleChoosedColor)
                .AddText(labelTxt, choosedTxtColor)
                .Build();

            AnimService.Instance.BuildAnim<UnchooseSlot>(gameObject)
                .AddImage(slotBgImg, baseBgColor)
                .AddImage(toggleBgImg, toggleBaseColor)
                .AddImage(toggleCheckmarkImg, toggleBaseColor)
                .AddText(labelTxt, baseTxtColor)
                .Build();
        }
    }
}