using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestProjectAppache
{
    public class PlayerController : MonoBehaviour
    {
        private Platform currentPlatform;
        public Action<Platform> ChangePlaftorm;

        public void Init(Platform startPlatform)
        {
            currentPlatform = startPlatform;
            gameObject.transform.position = currentPlatform.PositionPlayer.position;
            gameObject.transform.SetParent(currentPlatform.gameObject.transform);
        }

      
        private void MoveToPlatform()
        {
            currentPlatform = GameSpring.Instance.LevelGenerator.GetNextPlatform(currentPlatform);
            gameObject.transform.position = currentPlatform.PositionPlayer.position;
            gameObject.transform.SetParent(currentPlatform.gameObject.transform);

            if (ChangePlaftorm != null) ChangePlaftorm(currentPlatform);
        }

        public void Stretch()
        {
            Debug.Log("Strech tap");
            MoveToPlatform();
        }
    }
}
