
using System;
using DG.Tweening;
using UnityEngine;

namespace Game_camera
{
    public interface ICameraView
    {
        event Action onEndMove;
        event Action onEndScale;

        float Size { get; set; }

        void AnimateNewSize(float size);
        void AnimateNewPosition(Vector3 pos);
    }

    public class CameraView : MonoBehaviour, ICameraView
    {
        [SerializeField] private Camera camera;
        [SerializeField] private float timeForMove = 2.0f;

        public float Size { get => camera.orthographicSize; set => camera.orthographicSize = value; }

        public event Action onEndMove;
        public event Action onEndScale;

        public void AnimateNewPosition(Vector3 pos)
        {
            camera.transform.DOMove(pos, timeForMove)
                .OnComplete(
                    () =>
                    {
                        onEndMove?.Invoke();
                    }
                );
        }

        public void AnimateNewSize(float size)
        {
            camera.DOOrthoSize(size, timeForMove)
                .OnComplete(
                () =>
                {
                    onEndScale?.Invoke();
                }
                );
        }
    }
}