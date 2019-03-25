using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TestProjectAppache
{
    public class ScreenGame : GUIScreen
    {
        [Header("Screen Elements")]
        [SerializeField] private Button StretchPlayerButton;


        void Start()
        {
            StretchPlayerButton.onClick.RemoveAllListeners();
            StretchPlayerButton.onClick.AddListener(StretchPlayer);
        }

        private void StretchPlayer()
        {
            Debug.Log("Strech tap");
        }
    }
}
