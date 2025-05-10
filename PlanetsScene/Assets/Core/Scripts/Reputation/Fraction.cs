using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Reptutation_Screen
{
    public interface IFraction
    {
        Vector3 Size { get; }
        Texture Image { set; }
        string Name { set; get; }

        void SetReputation(int value);
        void SetVisibility(bool isVisible);
    }

    public class Fraction : MonoBehaviour, IFraction
    {
        [Header("Settings")]
        [SerializeField] private int minReputation;
        [SerializeField] private int maxReputation;
        [SerializeField] private float sizeOfOneBarPartX = 20.0f;

        [Header("Objs")]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text reputationDescText;
        [SerializeField] private RectTransform self;
        [SerializeField] private RawImage icon;
        [SerializeField] private RectTransform barFiled;
        [SerializeField] private RectTransform barPartPrefab;
        private List<GameObject> barParts;

        public Vector3 Size => new Vector3(self.localScale.x * self.sizeDelta.x, self.localScale.y * self.sizeDelta.y, 0.0f);
        public Texture Image { 
            set 
            {
                if (value == null)
                    return;

                icon.texture = value;
            } 
        }
        public string Name 
        { 
            set => nameText.text = value; 
            get => nameText.text;
        }

        public void SetVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        public void SetReputation(int reputation)
        {
            barParts.ForEach(x => x.SetActive(false));

            reputation = Mathf.Clamp(reputation, minReputation, maxReputation);

            float normVal = Mathf.Clamp01((float)(reputation - minReputation) / (float)(maxReputation - minReputation));
            int arrayMiddle = barParts.Count / 2;
            int arrayPoint;
            if (normVal < 0.5f)
            {
                arrayPoint = (int)(barParts.Count * Mathf.Abs(normVal - 0.051f));
                for (int i = arrayMiddle - 1; i >= arrayPoint; i--)
                {
                    barParts[i].SetActive(true);
                }
            }
            else
            {
                arrayPoint = (int)(Math.Round(barParts.Count * Mathf.Abs(normVal), 0));
                
                for (int i = arrayMiddle; i <= arrayPoint; i++)
                {
                    barParts[i].SetActive(true);
                }
            }

            SetReputationDesc(reputation);
        }

        //TODO: сделать так, чтобы всегда мы получали четное количество участков, или прост от какой-то средней точки спаунить все
        public void Awake()
        {
            barParts = new List<GameObject>();

            Vector3 spawnPos = barPartPrefab.position;
            int countOfObjs = (int)(barFiled.sizeDelta.x / sizeOfOneBarPartX);

            barPartPrefab.sizeDelta = new Vector2(sizeOfOneBarPartX, barPartPrefab.sizeDelta.y);
            for(int i = 0; i < countOfObjs; i++)
            {
                GameObject barPart = GameObject.Instantiate(barPartPrefab.gameObject, spawnPos, Quaternion.identity, barFiled.transform);
                barParts.Add(barPart);

                spawnPos.x += sizeOfOneBarPartX;
            }
        }

        private void SetReputationDesc(int reputation)
        {
            reputation = Mathf.Clamp(reputation, minReputation, maxReputation);

            if(reputation <= -75)
            {
                reputationDescText.text = "Ненависть";
            }
            else if(reputation <= -50 && reputation >= -74)
            {
                reputationDescText.text = "Вражда";
            }
            else if (reputation <= -25 && reputation >= -49)
            {
                reputationDescText.text = "Недоверие";
            }
            else if (reputation <= 24 && reputation >= -24)
            {
                reputationDescText.text = "Нейтралитет";
            }
            else if (reputation <= 49 && reputation >= 25)
            {
                reputationDescText.text = "Доверие";
            }
            else if (reputation <= 74 && reputation >= 50)
            {
                reputationDescText.text = "Дружба";
            }
            else
            {
                reputationDescText.text = "Почитание";
            }

        }
    }
}
