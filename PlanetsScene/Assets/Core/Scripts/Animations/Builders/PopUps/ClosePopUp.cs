
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Frontier_anim
{

    public class ClosePopUp : AnimBuilder
    {
        struct CanvasGroupAnimData
        {
            public CanvasGroup canvasGroup;
            public float startAlpha;
            public float endAlpha;

            public float startScale01;
            public float endScale01;

            public float playTime;
        }

        //background settings
        private Image bg;
        private float bgAlphaStart = 0.9058824f;
        private float bgAlphaEnd = 0.0f;
        private float timeForAppearBG = 0.1f;

        private static int ID = 0;

        private List<CanvasGroupAnimData> canvasGroups;

        protected override void InitAnim()
        {
            bg = null;

            if (canvasGroups == null)
            {
                canvasGroups = new List<CanvasGroupAnimData>();
            }
            else
            {
                canvasGroups.Clear();
            }
        }

        protected override Sequence EndBuild()
        {
            Sequence closeSeq = DOTween.Sequence();

            float maxAnimTime = 0.0f;
            foreach (CanvasGroupAnimData data in canvasGroups)
            {
                CanvasGroup group = data.canvasGroup;

                Vector3 startScale = group.transform.localScale * data.startScale01;
                Vector3 endScale = group.transform.localScale * data.endScale01;

                closeSeq.Insert(0.0f, group.DOFade(data.startAlpha, 0.0f));
                closeSeq.Insert(0.0f, group.transform.DOScale(startScale, 0.0f));

                closeSeq.Insert(0.0f, group.DOFade(data.endAlpha, data.playTime));
                closeSeq.Insert(0.0f, group.transform.DOScale(endScale, data.playTime));
                maxAnimTime = Mathf.Max(maxAnimTime, data.playTime);
            }

            if(bg != null)
            {
                closeSeq.Insert(0.0f, bg.DOFade(bgAlphaStart, 0.0f));
                closeSeq.Insert(maxAnimTime, bg.DOFade(bgAlphaEnd, timeForAppearBG));
            }

            return closeSeq;
        }

        public ClosePopUp SetImage(Image bgImg, float playAnimTime = 0.1f, float bgAlphaStart = 0.9058824f, float bgAlphaEnd = 0.0f)
        {
            bg = bgImg;
            this.timeForAppearBG = playAnimTime;

            this.bgAlphaStart = bgAlphaStart;
            this.bgAlphaEnd = bgAlphaEnd;

            return this;
        }

        public ClosePopUp AddCanvasGroup(CanvasGroup canvasGroup,
                                   float playAnimTime = 0.1f,
                                   float startAlpha = 1.0f, float endAlpha = 0.0f,
                                   float startScaleFactor01 = 1.0f, float endScaleFactor = 0.95f)
        {
            CanvasGroupAnimData animData = new CanvasGroupAnimData();
            animData.canvasGroup = canvasGroup;

            animData.playTime = playAnimTime;

            animData.startAlpha = startAlpha;
            animData.endAlpha = endAlpha;

            animData.startScale01 = startScaleFactor01;
            animData.endScale01 = endScaleFactor;

            canvasGroups.Add(animData);

            return this;
        }
    }
}