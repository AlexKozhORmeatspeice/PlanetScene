using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;

namespace Frontier_anim
{
    public class ChooseSlot : AnimBuilder
    {
        struct ImageData
        {
            public Image image;
            public float playTime;
            public Color color;
            public float alpha;
        }

        struct CanvasGroupData
        {
            public CanvasGroup canvasGroup;
            public float playTime;
            public float alpha;
        }

        struct TextData
        {
            public TMP_Text text;
            public Color color;
            public float playTime;
            public float alpha;
        }

        private const float BASE_PLAY_TIME = 0.2f;

        private List<ImageData> images;
        private List<CanvasGroupData> canvasGroups;
        private List<TextData> texts;

        private Color baseChoosedColor = new Color(0.796f, 0.992f, 1.0f);

        protected override void InitAnim()
        {
            if(images == null)
            {
                images = new List<ImageData>();     
            }
            else
            {
                images.Clear();
            }

            if (canvasGroups == null)
            {
                canvasGroups = new List<CanvasGroupData>();
            }
            else
            {
                canvasGroups.Clear();
            }

            if (texts == null)
            {
                texts = new List<TextData>();
            }
            else
            {
                texts.Clear();
            }
        }

        protected override Sequence EndBuild()
        {
            Sequence chooseSeq = DOTween.Sequence();

            foreach(ImageData imgData in images)
            {
                Image img = imgData.image;
                chooseSeq.Insert(0.0f, img.DOColor(imgData.color, imgData.playTime));
                chooseSeq.Insert(0.0f, img.DOFade(imgData.alpha, imgData.playTime));
            }

            foreach(CanvasGroupData canvasGroupData in canvasGroups)
            {
                CanvasGroup canvasGroup = canvasGroupData.canvasGroup;
                chooseSeq.Insert(0.0f, canvasGroup.DOFade(canvasGroupData.alpha, canvasGroupData.playTime));
            }

            foreach (TextData textData in texts)
            {
                TMP_Text text = textData.text;
                chooseSeq.Insert(0.0f, text.DOColor(textData.color, textData.playTime));
                chooseSeq.Insert(0.0f, text.DOFade(textData.alpha, textData.playTime));
            }

            return chooseSeq;
        }

        public ChooseSlot AddImage(Image image, Color color = default, float playTime = BASE_PLAY_TIME, float alpha = 1.0f)
        {
            ImageData imageData = new ImageData();
            
            Color endColor = color == default ? baseChoosedColor : color; 

            imageData.image = image;
            imageData.color = endColor;
            imageData.playTime = playTime;
            imageData.alpha = alpha;

            images.Add(imageData);

            return this;
        }

        public ChooseSlot AddCanvasGroup(CanvasGroup canvasGroup, float playTime = BASE_PLAY_TIME, float alpha = 1.0f)
        {
            CanvasGroupData canvasGroupData = new CanvasGroupData();

            canvasGroupData.canvasGroup = canvasGroup;
            canvasGroupData.playTime = playTime;
            canvasGroupData.alpha = alpha;

            canvasGroups.Add(canvasGroupData);

            return this;
        }

        public ChooseSlot AddText(TMP_Text text, Color color = default, float playTime = BASE_PLAY_TIME, float alpha = 1.0f)
        {
            TextData textData = new TextData();

            Color endColor = color == default ? baseChoosedColor : color;

            textData.text = text;
            textData.color = endColor;
            textData.playTime = playTime;
            textData.alpha = alpha;

            texts.Add(textData);

            return this;
        }
    }

}
