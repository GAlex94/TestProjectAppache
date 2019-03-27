using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestProjectAppache
{
    public class PlayerController : MonoBehaviour
    {
        public Action<Platform> ChangePlaftorm;
        private Platform currentPlatform;
        [SerializeField] private LineRenderer skinLineRenderer;
        [SerializeField] private float AddCountHight;
        [SerializeField] private float KoefHight;
        [SerializeField] private float OffesetMove = 0.2f;

        private float HightPlayer;

        public void Init(Platform startPlatform)
        {
            currentPlatform = startPlatform;
            gameObject.transform.position = currentPlatform.PositionPlayer.position;
            gameObject.transform.SetParent(currentPlatform.PositionPlayer.transform);
            HightPlayer = 1f;
            SizePlayer(1f);            
        }

        private void SizePlayer(float needPosition)
        {
            var step = needPosition / skinLineRenderer.positionCount;
            var position = Vector3.zero - Vector3.up * step;
            for (int i = 0; i < skinLineRenderer.positionCount; i++)
            {
                position += Vector3.up * step;
                skinLineRenderer.SetPosition(i, position);
            }
        }


        public void MoveToPlatform()
        {
            Debug.Log("Move to next platform");
            var nextPlatfrom = GameSpring.Instance.LevelGenerator.GetNextPlatform(currentPlatform);
            if (Math.Abs(Math.Abs(nextPlatfrom.PositionPlayer.position.x - currentPlatform.PositionPlayer.position.x) - HightPlayer) < OffesetMove)
            {
                GameSpring.Instance.AddScore(currentPlatform.Reward);
                MovePlayer(currentPlatform, nextPlatfrom);
            }
            else
            {
                if (GameManager.Instance.IsDebug)
                {
                    HightPlayer = 1;
                    SizePlayer(HightPlayer);
                }
                else
                {
                    GameSpring.Instance.GameOver();
                }
            }
        }

        public void Stretch()
        {
        //    Debug.Log("Strech press");
            HightPlayer += AddCountHight;
            SizePlayer(HightPlayer * KoefHight);
        }


        private void MovePlayer(Platform currentPlatform, Platform nextPlatfrom)
        {
            currentPlatform = nextPlatfrom;
            gameObject.transform.position = currentPlatform.PositionPlayer.position;
            gameObject.transform.SetParent(currentPlatform.PositionPlayer);
            HightPlayer = 1;
            SizePlayer(HightPlayer);
            if (ChangePlaftorm != null) ChangePlaftorm(currentPlatform);
        }
    }
}
