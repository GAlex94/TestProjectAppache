using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TestProjectAppache
{
    public class ScreenGameOver : GUIScreen
    {
        [Header("Screen Elements")]
        [SerializeField] private Button RestartButton;
        [SerializeField] private TMPro.TextMeshProUGUI rewardText;

        void Start()
        {
            RestartButton.onClick.RemoveAllListeners();
            RestartButton.onClick.AddListener(() => GameManager.Instance.Restart());
        }

        private void OnEnable()
        {
            rewardText.text = string.Format("YOU COLLECTED {0} COIINS", GameSpring.Instance.CurrentScore);
        }
    }
}

