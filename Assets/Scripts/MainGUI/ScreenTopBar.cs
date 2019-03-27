using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestProjectAppache
{
    public class ScreenTopBar : GUIScreen
    {
        [SerializeField]
        private MoneyListener moneyText;
        [SerializeField]
        private TextMeshProUGUI scoreText;

        public void OnEnable()
        {
            GameSpring.Instance.ChangeScore += UpdateScoreText;
            UpdateScoreText(GameSpring.Instance.CurrentScore);
        }

        public void OnDisable()
        {
         //   GameSpring.Instance.ChangeScore -= UpdateScoreText;
        }

        private void UpdateScoreText(int score)
        {
            scoreText.text = string.Format("SCORE {0}", score);
        }
    }
}
