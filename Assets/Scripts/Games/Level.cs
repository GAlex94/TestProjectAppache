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
        private Vector3 firstPostion;
        private bool isInit;

        public void Init(Transform positionStart)
        {
            targetPosition = positionStart.position;
            isInit = true;
        }

        [ContextMenu("Move")]
        public void Move(Transform currentPlatform)
        {
            if (!isInit || currentPlatform == null)
                return;

            var offset = gameObject.transform.position.x - Math.Abs(targetPosition.x - currentPlatform.position.x);
            transform.LeanMoveX(offset, speedMove);

            firstPostion.x -= offset;
            Debug.LogError("offset" + offset);
            if (Math.Abs(firstPostion.x + offset) > 5)
            {
                Debug.Log("Need generate Back");
                firstPostion.x = gameObject.transform.position.x;
                GameSpring.Instance.LevelGenerator.GenerateNewBack();
            }
        }
    }
}