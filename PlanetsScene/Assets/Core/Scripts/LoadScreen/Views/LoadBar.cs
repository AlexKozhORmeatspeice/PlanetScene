using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Save_screen
{
    public interface ILoadBar
    {
        void ChangeNormValue(float t01);
    }

    public class LoadBar : MonoBehaviour, ILoadBar
    {
        [SerializeField] private TMP_Text loadTitle;
        [SerializeField] private TMP_Text valueText;
        [SerializeField] private Slider valueSlider;

        public void ChangeNormValue(float t01)
        {
            t01 = Mathf.Clamp01(t01);

            valueSlider.value = t01;
            valueText.text = Mathf.Round(100.0f * t01).ToString() + "%";
        }
    }
}
