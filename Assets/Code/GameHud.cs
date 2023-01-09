using System;
using TMPro;
using UnityEngine;

namespace Code
{
    public class GameHud : MonoBehaviour
    {
        public TextMeshProUGUI text;

        public GameObject missionObject;
        public TextMeshProUGUI missionText;

        public float missionTextDuration;

        public int targetSpice;

        private void Awake()
        {
            missionObject.SetActive(false);
        }

        public void SetMission(string text)
        {
            missionObject.SetActive(true);
            missionText.text = text;
            LeanTween.delayedCall(gameObject, missionTextDuration, delegate(object o)
            {
                missionObject.SetActive(false);
            });
        }
        
        public void UpdateSpice(float spice)
        {
            text.text = $"{spice:0}/{targetSpice}";
        }
    }
}