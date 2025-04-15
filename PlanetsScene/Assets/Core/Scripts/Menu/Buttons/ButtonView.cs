using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Menu
{
    public interface IButtonView
    {
        event UnityAction onClick;
        void LerpObjectState(float t01);

        Vector2 Size { get; }
        Vector3 Position { get; }
        Vector3 ScreenPosition { get; }
        float OuterDistMultilpier { get; }
    }

    public class ButtonView : MonoBehaviour, IButtonView
    {
        [Header("Settings")]
        [SerializeField] private Color visibleTextColor;
        [SerializeField] private Color hideTextColor;
        [SerializeField] private float outerDistMultilpier = 3.0f;

        [Header("Objs")]
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image underlineImg1;
        [SerializeField] private Image underlineImg2;

        public Vector3 Position => button.transform.position;

        public Vector3 ScreenPosition => Camera.main.WorldToScreenPoint(transform.position);

        public float OuterDistMultilpier => outerDistMultilpier;

        public Vector2 Size => rectTransform.sizeDelta;

        public event UnityAction onClick
        {
            add => button.onClick.AddListener(value);
            remove => button.onClick.RemoveListener(value);
        }

        public void LerpObjectState(float t01)
        {
            t01 = Mathf.Clamp01(t01);

            underlineImg1.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), t01);
            underlineImg2.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), t01);

            text.color = Color.Lerp(hideTextColor, visibleTextColor, t01);
        }
    }
}
