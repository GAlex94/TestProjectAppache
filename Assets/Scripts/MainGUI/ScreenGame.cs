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
        [SerializeField] private ButtonStretch StretchPlayerButton;


        void Start()
        {
            StretchPlayerButton.Init(StretchPlayer, MovePlayer);
        }

        private void StretchPlayer()
        {
            GameSpring.Instance.PlayerController.Stretch();
        }

        private void MovePlayer()
        {
            GameSpring.Instance.PlayerController.MoveToPlatform();
        }
    }
}
