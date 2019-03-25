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
        [SerializeField]
        private Button startGameButton;


        void Start()
        {
            startGameButton.onClick.RemoveAllListeners();
            startGameButton.onClick.AddListener(StartGame());
        }

        private UnityAction StartGame()
        {
            throw new System.NotImplementedException();
        }
    }
}
