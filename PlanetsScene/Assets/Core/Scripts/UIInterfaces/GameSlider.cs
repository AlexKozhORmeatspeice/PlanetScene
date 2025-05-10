

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game_UI
{
    public interface ISlider
    {
        event UnityAction<float> onChangeValue;

        string Text { set; }
        float Value { get; }
    }

    public class GameSlider : MonoBehaviour, ISlider
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text text;

        public string Text { set => text.text = value; }

        public float Value => slider.value;

        public event UnityAction<float> onChangeValue
        {
            add => slider.onValueChanged.AddListener(value);
            remove => slider.onValueChanged.RemoveListener(value);
        }
    }
}

