using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Game_UI
{
    public interface IGameToggle
    {
        event UnityAction<bool> onValueChanged;
    }

    public class GameToggle : MonoBehaviour, IGameToggle
    {
        [SerializeField] private Toggle toggle;

        public event UnityAction<bool> onValueChanged
        {
            add => toggle.onValueChanged.AddListener(value);
            remove => toggle.onValueChanged.RemoveListener(value);
        }
    }

}
