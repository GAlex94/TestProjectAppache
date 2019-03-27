using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestProjectAppache
{
    public class MovePlatform : Platform
    {
        [SerializeField] private float speedMove = 5;
        [SerializeField] private float distanceMove;
        [SerializeField] private Transform platform;

        private Vector3 directionMove;


        private void OnEnable()
        {
            platform.localPosition = Vector3.zero;

            switch (TypePlatform)
            {
                case TypePlatformEnum.Default:
                    break;
                case TypePlatformEnum.VerticalMove:
                    directionMove = Vector3.up;
                    break;
                case TypePlatformEnum.Horizontal:
                    directionMove = Vector3.right;
                    break;
                default:
                    break;
            }
            directionMove *= UnityEngine.Random.Range(0, 1) == 0 ? -1* distanceMove : distanceMove;
        }

        private void Update()
        {
            if (TypePlatform == TypePlatformEnum.Horizontal)
            {
                if (Mathf.Abs(platform.localPosition.x - directionMove.x) > 0.01)
                {
                    platform.localPosition= new Vector3(Mathf.Lerp(platform.localPosition.x, directionMove.x, speedMove * Time.deltaTime), 0, 0);
                }
                else
                {
                    directionMove *= -1;
                }
            }

            if (TypePlatform == TypePlatformEnum.VerticalMove)
            {
                if (Mathf.Abs(platform.localPosition.y - directionMove.y) > 0.01)
                {
                    platform.localPosition = new Vector3(0, Mathf.Lerp(platform.localPosition.y, directionMove.y, speedMove * Time.deltaTime), 0);
                }
                else
                {
                    directionMove *= -1;
                }
            }



        }

    }
}