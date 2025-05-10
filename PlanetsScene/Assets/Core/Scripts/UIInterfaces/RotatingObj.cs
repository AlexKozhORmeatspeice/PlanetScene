using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace Game_UI
{
    public interface IRotatingObject
    {
        void StartAnim();
    }

    public class RotatingObj : MonoBehaviour, IRotatingObject
    {
        [SerializeField] private float timeFullCircle = 3.0f;
        [SerializeField] private Vector3 startRot = new Vector3(0.0f, 0.0f, 0.0f);
        [SerializeField] private Vector3 endRot = new Vector3(0.0f, 0.0f, -360.0f);
        private RectTransform selfTransform;
        private bool animIsActive = false;

        public void StartAnim()
        {
            animIsActive = true;
        }

        private void Awake()
        {
            selfTransform = GetComponent<RectTransform>();

            SetNewRotationByTime(Time.time);
        }

        void FixedUpdate()
        {
            if (!animIsActive)
                return;

            SetNewRotationByTime(Time.time);
        }

        private void SetNewRotationByTime(float time)
        {
            float t = time / timeFullCircle - Mathf.Floor(time / timeFullCircle);

            Vector3 newRot = startRot * t + endRot * (1.0f - t);
            selfTransform.rotation = Quaternion.Euler(newRot);
        }
    }

}