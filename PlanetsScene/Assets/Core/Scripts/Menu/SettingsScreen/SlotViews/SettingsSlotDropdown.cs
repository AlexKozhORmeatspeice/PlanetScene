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
    public interface ISettingsSlotDropdown : ISettingsSlot
    {
        event UnityAction<int> onChangeDropdownValue;
        public void AnimateHideDropdown();
        public void AnimateChoosed();
        public void AnimateUnchoosed();
    }

    public class SettingsSlotDropdown : MonoBehaviour, ISettingsSlotDropdown
    {
        [Header("BG colors")]
        [SerializeField] private Color baseBgColor;
        [SerializeField] private Color choosedBgColor;

        [Header("Text color")]
        [SerializeField] private Color baseTxtColor;
        [SerializeField] private Color choosedTxtColor;

        [Header("Objs")]
        [SerializeField] private DropdownList dropdown;
        [SerializeField] private Image slotBgImg;
        [SerializeField] private TMP_Text dropdownTxt;
        [SerializeField] private TMP_Text dropdownElemTxt;

        public event UnityAction<int> onChangeDropdownValue
        {
            add => dropdown.onChangeValue += value;
            remove => dropdown.onChangeValue -= value;
        }

        public void AnimateChoosed()
        {
            AnimService.Instance.PlayAnim<ChooseSlot>(gameObject);
        }

        public void AnimateHideDropdown()
        {
            dropdown.AnimateHide();
        }

        public void AnimateUnchoosed()
        {
            AnimService.Instance.PlayAnim<UnchooseSlot>(gameObject);
        }

        private void Awake()
        {
            AnimService.Instance.BuildAnim<ChooseSlot>(gameObject)
                .AddImage(slotBgImg, choosedBgColor)
                .AddText(dropdownTxt, choosedTxtColor)
                .AddText(dropdownElemTxt, choosedTxtColor)
                .Build();

            AnimService.Instance.BuildAnim<UnchooseSlot>(gameObject)
                .AddImage(slotBgImg, baseBgColor)
                .AddText(dropdownTxt, baseTxtColor)
                .AddText(dropdownElemTxt, baseTxtColor)
                .Build();
        }
    }
}