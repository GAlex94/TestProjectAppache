using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TestProjectAppache
{
    public class ScreenStartGame : GUIScreen
    {
        [Header("Screen Elements")]
        [SerializeField] private Button startGameButton;


        void Start()
        {
            startGameButton.onClick.RemoveAllListeners();
            startGameButton.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            GUIController.Instance.ShowScreen<ScreenGame>();
            Hide();
        }
    }
}
