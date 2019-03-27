using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestProjectAppache
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private float speedMove;

        private Vector3 targetPosition;
        private Vector3 positionBackGround;
        private bool isInit;

        public void Init(Transform positionStart)
        {
            targetPosition = positionStart.position;
            positionBackGround = Vector3.right * 5;
            isInit = true;
        }
     
        public void Move(Transform currentPlatform)
        {
            if (!isInit || currentPlatform == null)
                return;

            var offset = gameObject.transform.position.x - Math.Abs(targetPosition.x - currentPlatform.position.x);
            transform.LeanMoveX(offset, speedMove);

            if (positionBackGround.x - Math.Abs(gameObject.transform.position.x) < 5)
            {
              //  Debug.Log("Need generate Back");
                positionBackGround.x += 30;
                GameSpring.Instance.LevelGenerator.GenerateNewBack();
            }
        }
    }
}