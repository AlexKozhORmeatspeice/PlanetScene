using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Frontier_anim
{
    public class OpenPopUp : AnimBuilder
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
        private float bgAlphaStart = 0.0f;
        private float bgAlphaEnd = 0.9058824f;
        private float timeForAppearBG = 0.2f;

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
            Sequence openSeq = DOTween.Sequence();
            
            if(bg != null)
            {
                openSeq.Append(bg.DOFade(bgAlphaStart, 0.0f));
                openSeq.Append(bg.DOFade(bgAlphaEnd, timeForAppearBG));
            }

            foreach (CanvasGroupAnimData data in canvasGroups)
            {
                CanvasGroup group = data.canvasGroup;

                Vector3 startScale = group.transform.localScale * data.startScale01;
                Vector3 endScale = group.transform.localScale * data.endScale01;

                openSeq.Insert(0.0f, group.DOFade(data.startAlpha, 0.0f));
                openSeq.Insert(0.0f, group.transform.DOScale(startScale, 0.0f));

                openSeq.Insert(timeForAppearBG, group.DOFade(data.endAlpha, data.playTime));
                openSeq.Insert(timeForAppearBG, group.transform.DOScale(endScale, data.playTime));
            }

            return openSeq;
        }

        public OpenPopUp SetImage(Image bgImg, float playAnimTime = 0.1f, float bgAlphaStart = 0.0f, float bgAlphaEnd = 0.9058824f)
        {
            bg = bgImg;
            this.timeForAppearBG = playAnimTime;
            
            this.bgAlphaStart = bgAlphaStart;
            this.bgAlphaEnd = bgAlphaEnd;

            return this;
        }

        public OpenPopUp AddCanvasGroup(CanvasGroup canvasGroup, 
                                   float playAnimTime = 0.1f,
                                   float startAlpha = 0.0f, float endAlpha = 1.0f, 
                                   float startScaleFactor01 = 0.95f, float endScaleFactor = 1.0f)
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