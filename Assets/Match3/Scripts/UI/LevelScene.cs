using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace Match3.Scripts.UI
{
    public class LevelScene : MonoBehaviour
    {
        private int currentLevel;
        [SerializeField] private TextMeshProUGUI LevelText;
        [SerializeField] private TextMeshProUGUI LifeCounter;
        [SerializeField] private TextMeshProUGUI LifeTimer;
        private int timer = 900;
        private void Awake()
        {
            LinkKeeper.currentLevel = MatchManager.Instance.gameData.currentLevel;
            currentLevel = LinkKeeper.currentLevel;
            LevelText.text = $"Уровень { currentLevel }";
            SetLifeCount();
            UpdateLifeTimerText();
        }

        private void UpdateLifeTimerText()
        {
            if (MatchManager.Instance.gameData.lifesCount == MatchManager.Instance.gameData.maxLifesCount)
            {
                LifeTimer.text = $" Полные";
                timer = 900;
            }
            else
            {
                SetLifeTimer(timer);
            }
        }

        public void SetLifeCount()
        {
            LifeCounter.text = string.Format("{0}", MatchManager.Instance.gameData.lifesCount);
        }

        public void SetLifeTimer(int amount)
        {
            var timeSpan = TimeSpan.FromSeconds(amount);
            LifeTimer.text= string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}
