using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestProjectAppache
{
    public class PlayerController : MonoBehaviour
    {
        private Platform currentPlatform;
        [SerializeField] private BoxCollider2D playerSkin;
        [SerializeField] private LineRenderer skinLineRenderer;
        public Action<Platform> ChangePlaftorm;

        public void Init(Platform startPlatform)
        {
            currentPlatform = startPlatform;
            gameObject.transform.position = currentPlatform.PositionPlayer.position;
            gameObject.transform.SetParent(currentPlatform.PositionPlayer.transform);
            HightPlayer = 1f;
            SizePlayer(1f);


            //       playerSkin.transform.localScale = new Vector3(1, 0.1f, 1);
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
       //     Debug.LogError(Math.Abs(Math.Abs(nextPlatfrom.transform.position.x - currentPlatform.transform.position.x) - HightPlayer));
            if (Math.Abs(Math.Abs(nextPlatfrom.PositionPlayer.position.x - currentPlatform.PositionPlayer.position.x) - HightPlayer) < OffesetMove)
            {
                currentPlatform = nextPlatfrom;
                gameObject.transform.position = currentPlatform.PositionPlayer.position;
                gameObject.transform.SetParent(currentPlatform.PositionPlayer);
                GameSpring.Instance.AddScore(currentPlatform.Reward);
                HightPlayer = 1;
                SizePlayer(HightPlayer);
                //       playerSkin.transform.localScale = new Vector3(1, 0.1f, 1);
                if (ChangePlaftorm != null) ChangePlaftorm(currentPlatform);
            }
            else
            {
                HightPlayer = 1;
           //     playerSkin.transform.localScale = new Vector3(1, 0.1f, 1);
                //      GameSpring.Instance.GameOver();
            }

           
        }


        public float HightPlayer;
        public float AddCountHight;
        public float KoefHight;
        public float OffesetMove = 0.2f;

        public void Stretch()
        {
            Debug.Log("Strech press + "  + playerSkin.bounds.size.y);
            HightPlayer += AddCountHight;
            SizePlayer(HightPlayer * KoefHight);
            var nextPlatfrom = GameSpring.Instance.LevelGenerator.GetNextPlatform(currentPlatform);
            Debug.LogError(Math.Abs(Math.Abs(nextPlatfrom.PositionPlayer.position.x - currentPlatform.PositionPlayer.position.x) - playerSkin.bounds.size.y));
            // MoveToPlatform();
        }
    }
}
