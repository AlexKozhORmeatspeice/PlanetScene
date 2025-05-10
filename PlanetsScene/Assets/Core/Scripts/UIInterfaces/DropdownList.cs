

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game_UI
{
    public interface IDropdownList
    {
        event UnityAction<int> onChangeValue;
        public void AnimateHide();
    }

    public class DropdownList : MonoBehaviour, IDropdownList
    {
        [SerializeField] private TMP_Dropdown dropdown;
        
        public event UnityAction<int> onChangeValue
        {
            add => dropdown.onValueChanged.AddListener(value);
            remove => dropdown.onValueChanged.RemoveListener(value);
        }

        public void AnimateHide()
        {
            dropdown.Hide();
        }

        private void Awake()
        {
        }
    }
}

