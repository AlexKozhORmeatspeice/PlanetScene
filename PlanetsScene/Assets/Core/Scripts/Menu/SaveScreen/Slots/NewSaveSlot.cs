using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Frontier_anim;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Save_screen
{
    public interface INewSaveSlot
    {
        event UnityAction onSaveClick;

        ITextButton SaveButton { get; }

        string Name { set; }

        Vector3 Size { get; }
        Vector3 WolrdPosition { get; set; }
        float DistToDetectMouseY { get; }

        void SetVisibility(bool isVIsible);
        void AnimateChoosed();
        void AnimateNotChoosed();

        void Destroy();
    }

    public class NewSaveSlot : MonoBehaviour, INewSaveSlot
    {
        [Header("Settings")]
        [SerializeField] private Color bgBaseColor;
        [SerializeField] private Color bgChoosedColor;

        [SerializeField] private Color textBaseColor;
        [SerializeField] private Color textChoosedColor;

        [SerializeField] private Color outlineBaseColor;
        [SerializeField] private Color outlineChoosedColor;

        [SerializeField] private float distToDetectMouseY;

        [Header("objs")]
        [SerializeField] private TextButton saveButton;
        [SerializeField] private RectTransform self;
        [SerializeField] private Image bg;
        [SerializeField] private Image outline;

        [SerializeField] private TMP_Text nameText;

        public event UnityAction onSaveClick
        {
            add => saveButton.onClick += value;
            remove => saveButton.onClick -= value;
        }

        public Vector3 Size => new Vector3(self.localScale.x * self.sizeDelta.x, self.localScale.y * self.sizeDelta.y, 0.0f);

        public Vector3 WolrdPosition
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }

        public float DistToDetectMouseY => distToDetectMouseY;

        public ITextButton SaveButton => saveButton;

        public string Name { set => nameText.text = value; }

       
        public void SetVisibility(bool isVIsible)
        {
            gameObject.SetActive(isVIsible);
        }


        public void ChangeColorByNormValue(float t01)
        {
            t01 = Mathf.Clamp01(t01);

            bg.color = Color.Lerp(bgBaseColor, bgChoosedColor, t01);
            outline.color = Color.Lerp(outlineBaseColor, outlineChoosedColor, t01);

            nameText.color = Color.Lerp(textBaseColor, textChoosedColor, t01);

        }

        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }

        public void AnimateChoosed()
        {
            AnimService.Instance.PlayAnim<ChooseSlot>(gameObject);
        }

        public void AnimateNotChoosed()
        {
            AnimService.Instance.PlayAnim<UnchooseSlot>(gameObject);
        }

        private void Awake()
        {
            AnimService.Instance.BuildAnim<ChooseSlot>(gameObject)
                .AddImage(bg, bgChoosedColor)
                .AddImage(outline, outlineChoosedColor)
                .AddText(nameText, textChoosedColor)
                .Build();

            AnimService.Instance.BuildAnim<UnchooseSlot>(gameObject)
                .AddImage(bg, bgBaseColor)
                .AddImage(outline, outlineBaseColor)
                .AddText(nameText, textBaseColor)
                .Build();
        }
    }
}

