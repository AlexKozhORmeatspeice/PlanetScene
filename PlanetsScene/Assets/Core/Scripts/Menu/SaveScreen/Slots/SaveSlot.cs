using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using DG.Tweening;
using Frontier_anim;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Save_screen
{
    public interface ISaveSlot
    {
        event UnityAction onRewriteClick;
        event UnityAction onDeleteClick;
        event Action onEndAnimDelete;
        ITextButton RewriteButton { get; }
        IIconButton DeleteButton { get; }

        string Name { set; }
        string Chapter { set; }
        string ID { set; }
        string Date { set; }

        Vector3 Size { get; }
        Vector3 WolrdPosition { get; set; }
        float DistToDetectMouseY { get; }

        void SetVisibility(bool isVIsible);

        void AnimateChoosed();
        void AnimateNotChoosed();

        void AnimateCreation();
        void AnimateDelete();

        SaveInfo Info { get; set; }

        void Destroy();
    }

    public class SaveSlot : MonoBehaviour, ISaveSlot
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
        [SerializeField] private CanvasGroup selfCanvasGroup;
        [SerializeField] private TextButton rewriteButton;
        [SerializeField] private IconButton deleteButton;
        [SerializeField] private RectTransform selfRect;
        [SerializeField] private Image bg;
        [SerializeField] private Image outline;

        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text chapterText;
        [SerializeField] private TMP_Text IDText;
        [SerializeField] private TMP_Text dateText;

        private SaveInfo info;

        public event Action onEndAnimDelete;

        public event UnityAction onRewriteClick
        {
            add => rewriteButton.onClick += value;
            remove => rewriteButton.onClick -= value;
        }

        public event UnityAction onDeleteClick
        {
            add => deleteButton.onClick += value;
            remove => deleteButton.onClick -= value;
        }

        public Vector3 Size => new Vector3(selfRect.localScale.x * selfRect.sizeDelta.x, selfRect.localScale.y * selfRect.sizeDelta.y, 0.0f);

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

        public ITextButton RewriteButton => rewriteButton;

        public IIconButton DeleteButton => deleteButton;

        public string Name { set => nameText.text = value; }
        public string Chapter { set => chapterText.text = value; }
        public string ID { set => IDText.text = value; }
        public string Date { set => dateText.text = value; }
        public SaveInfo Info 
        { 
            get => info;
            set
            {
                info = value;

                Name = info.name;
                Chapter = info.chapter;
                Date = info.date;
            }
        }

        public void SetVisibility(bool isVIsible)
        {
            gameObject.SetActive(isVIsible);
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

        public void AnimateCreation()
        {
            AnimService.Instance.PlayAnim<CreateSlot>(gameObject);
        }

        public void AnimateDelete()
        {
            AnimService.Instance.PlayAnim<DeleteSlot>(gameObject);
        }

        private void Awake()
        {
            AnimService.Instance.BuildAnim<ChooseSlot>(gameObject)
                .AddImage(bg, bgChoosedColor)
                .AddImage(outline, outlineChoosedColor)
                .AddText(nameText, textChoosedColor)
                .AddText(chapterText, textChoosedColor)
                .AddText(IDText, textChoosedColor)
                .Build();

            AnimService.Instance.BuildAnim<UnchooseSlot>(gameObject)
                .AddImage(bg, bgBaseColor)
                .AddImage(outline, outlineBaseColor)
                .AddText(nameText, textBaseColor)
                .AddText(chapterText, textBaseColor)
                .AddText(IDText, textBaseColor)
                .Build();

            AnimService.Instance.BuildAnim<CreateSlot>(gameObject)
                .SetCanvasGroup(selfCanvasGroup)
                .SetRectTransform(selfRect)
                .Build();

            AnimService.Instance.BuildAnim<DeleteSlot>(gameObject)
                .SetCanvasGroup(selfCanvasGroup)
                .SetRectTransform(selfRect)
                .SetCallback(InvokeEndDelete)
                .Build();
        }

        private void InvokeEndDelete()
        {
            onEndAnimDelete?.Invoke();
        }
    }
}

