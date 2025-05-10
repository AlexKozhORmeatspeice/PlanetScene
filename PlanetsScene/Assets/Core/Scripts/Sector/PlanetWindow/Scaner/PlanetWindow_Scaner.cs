using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace Planet_Window
{
    public interface IPlanetWindow_Scaner
    {
        bool isVisible { set; }
        bool scanerFiledIsVisible { set; }
        Texture Texture { set; }
        void ChangeSize(float t); // t == 0 - minSize, t == 1 - maxSize
        float SpeedOfDecrise { get; }
        float MaxMouseSpeed { get; }
    }

    public class PlanetWindow_Scaner : MonoBehaviour, IPlanetWindow_Scaner, IDisposable
    {
        [Header("Settings")]
        [SerializeField] private RawImage image;
        [SerializeField] private Vector2 maxSize;
        [SerializeField] private Vector2 minSize;
        [Range(0.01f, 3.0f)][SerializeField] private float speedOfDecrise = 1.0f;
        [Range(0.01f, 60.0f)][SerializeField] private float maxMouseSpeed = 0.01f;

        [Header("Objs")]
        [SerializeField] private RectTransform rectTransform;

        public float SpeedOfDecrise => speedOfDecrise;

        public float MaxMouseSpeed => maxMouseSpeed;

        public Texture Texture { set => image.texture = value; }
        public bool isVisible { set => gameObject.SetActive(value); }
        public bool scanerFiledIsVisible { set => image.gameObject.SetActive(value); }

        public void ChangeSize(float t01)
        {
            if (t01 == float.NaN)
                return;

            t01 = Mathf.Clamp01(t01);

            rectTransform.sizeDelta = Vector2.Lerp(minSize, maxSize, t01);
        }

        public void Dispose()
        {
            rectTransform.sizeDelta = minSize;
        }


        public void Awake()
        {
            LoadData();
        }

        private void LoadData()
        {
            string path = Application.dataPath + "/ScanerData.json";
            ScanerData scanerData;
            if (File.Exists(path))
            { 
                scanerData = JsonUtility.FromJson<ScanerData>(File.ReadAllText(path));
            }
            else
            {
                scanerData = new ScanerData();
                scanerData.minSquareSize = new Vector2(60.0f, 60.0f);
                scanerData.maxSquareSize = new Vector2(800.0f, 800.0f);
                scanerData.maxMouseSpeed = 15.0f;
                scanerData.speedOfDecrise = 0.25f;

                string json = JsonUtility.ToJson(scanerData);
                
                Debug.Log(path);
                File.WriteAllText(path, json);
            }

            minSize = scanerData.minSquareSize;
            maxSize = scanerData.maxSquareSize;
            maxMouseSpeed = scanerData.maxMouseSpeed;
            speedOfDecrise = scanerData.speedOfDecrise;
        }
        
        public class ScanerData
        {
            public float maxMouseSpeed;
            public float speedOfDecrise;
            public Vector2 minSquareSize;
            public Vector2 maxSquareSize;
        }
    }
    
    

}


