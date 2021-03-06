﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        [SerializeField] private float HightPlayer;
        [SerializeField] private bool isStretch;




        [Header("Param move")] [SerializeField]
        private float OffesetMove = 0.2f;

        [SerializeField] private float speed = 1;
        [SerializeField] private float speedMoveLine = 1;
        [SerializeField] private float speedLerp = 1;
        [SerializeField] private float defaultSpeedLerp = 1;
        [SerializeField] private float koef = 1.2f;



        private int linePointEnd;
        private Vector3 center;
        private Vector3 startPos;
        private float Radius;
        private bool step0;
        private bool step1;
        private bool step2;
        private bool step3;
        private bool step4;
        private bool step5;
        private bool step6;
        private bool step7;
        private float alfa = 0;

        public void Init(Platform startPlatform)
        {
            currentPlatform = startPlatform;
            gameObject.transform.position = currentPlatform.PositionPlayer.position;
            gameObject.transform.SetParent(currentPlatform.PositionPlayer.transform);
            HightPlayer = 1f;
            SizePlayer(1f);
            isStretch = false;
        }

        public void CanStretch()
        {
            isStretch = true;
        }

        private void SizePlayer(float needPosition)
        {
            var step = needPosition / (skinLineRenderer.positionCount - 1);
            var position = Vector3.zero;
            for (int i = 0; i < skinLineRenderer.positionCount; i++)
            {
                skinLineRenderer.SetPosition(i, position);
                position += Vector3.up * step;
            }
        }

        private void Update()
        {
            if (isStretch)
            {
                Stretch();
            }
        }


        public void MoveToPlatform()
        {
            if (!isStretch)
            {
                return;
            }

            Debug.Log("Move to next platform");
            isStretch = false;
            var nextPlatfrom = GameSpring.Instance.LevelGenerator.GetNextPlatform(currentPlatform);
            if (Math.Abs(Math.Abs(nextPlatfrom.PositionPlayer.position.x - currentPlatform.PositionPlayer.position.x) -
                         HightPlayer) < OffesetMove)
            {
                GameSpring.Instance.AddScore(currentPlatform.Reward);
                MovePlayer(nextPlatfrom);
            }
            else
            {
                var obj = new GameObject();
                obj.transform.position = new Vector3(currentPlatform.PositionPlayer.position.x + HightPlayer,
                    nextPlatfrom.PositionPlayer.position.y);

                if (GameManager.Instance.IsDebug)
                {
                    HightPlayer = 1;
                    SizePlayer(HightPlayer);
                    CanStretch();
                }
                else
                {
                    StartCoroutine(GameOVer());
                    //GameSpring.Instance.GameOver();
                }
            }
        }

        private IEnumerator GameOVer()
        {
            StartCoroutine(Moving(new Vector3(currentPlatform.PositionPlayer.position.x + HightPlayer,
                currentPlatform.PositionPlayer.position.y - 10f, currentPlatform.PositionPlayer.position.z)));
            yield return new WaitForSecondsRealtime(1f);
            GameSpring.Instance.GameOver();
        }

        public void Stretch()
        {
            HightPlayer += AddCountHight;
            SizePlayer(HightPlayer * KoefHight);
        }
#if UNITY_EDITOR
        [ContextMenu("Move")]
        public void TEst2()
        {
            var nextPlatfrom = GameSpring.Instance.LevelGenerator.GetNextPlatform(currentPlatform);
            MovePlayer(nextPlatfrom);
        }
#endif

        private void MovePlayer(Platform nextPlatfrom)
        {
            StartCoroutine(Moving(nextPlatfrom.PositionPlayer.position));
        }

        IEnumerator Moving(Vector3 nextPosition)
        {
            yield return null;
            linePointEnd = skinLineRenderer.positionCount - 1;
            var Platfrom2 = transform.InverseTransformPoint(nextPosition);
            var d = (Platfrom2 - skinLineRenderer.GetPosition(linePointEnd));
            center = skinLineRenderer.GetPosition(linePointEnd) + d / 2;
            Radius = d.magnitude / 2;
            //     Debug.Log((skinLineRenderer.GetPosition(linePointEnd).x - center.x) / Radius);
            alfa = Mathf.Asin((skinLineRenderer.GetPosition(linePointEnd).x - center.x) / Radius);
            startPos = skinLineRenderer.GetPosition(linePointEnd);

            step0 = true;
            yield return null;

            while (step0 && Mathf.Abs(skinLineRenderer.GetPosition(linePointEnd).y - (startPos.y - 0.2f)) > 0.1f)
            {
                yield return new WaitForFixedUpdate();
                skinLineRenderer.SetPosition(linePointEnd,
                    Vector3.MoveTowards(skinLineRenderer.GetPosition(linePointEnd), startPos - Vector3.up * 0.2f,
                        Time.deltaTime * speedMoveLine));
            }

            while (step0 && Mathf.Abs(skinLineRenderer.GetPosition(linePointEnd).y - startPos.y) > 0.1f)
            {
                yield return new WaitForFixedUpdate();
                skinLineRenderer.SetPosition(linePointEnd,
                    Vector3.MoveTowards(skinLineRenderer.GetPosition(linePointEnd), startPos,
                        Time.deltaTime * speedMoveLine));
            }


            step1 = true;
            yield return null;
            defaultSpeedLerp = speedLerp;
            while (step1 && Mathf.Abs(skinLineRenderer.GetPosition(linePointEnd).x - Platfrom2.x) > 0.1f)
            {
                yield return new WaitForFixedUpdate();
                alfa += Time.deltaTime * speed;
                var x = Radius * Mathf.Sin(alfa);
                var y = Radius * Mathf.Cos(alfa);
                skinLineRenderer.SetPosition(linePointEnd, new Vector3(x, y) + center);

                for (int i = skinLineRenderer.positionCount - 2; i > 0; i--)
                {
                    skinLineRenderer.SetPosition(i,
                        Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i + 1),
                            Time.deltaTime * skinLineRenderer.positionCount * speedLerp));
                    speedLerp /= koef;
                }

                speedLerp = defaultSpeedLerp;
            }


            step2 = true;

            while (step2 && Mathf.Abs(skinLineRenderer.GetPosition(linePointEnd).y - Platfrom2.y) > 0.1f)
            {
                yield return new WaitForFixedUpdate();
                skinLineRenderer.SetPosition(linePointEnd,
                    Vector3.MoveTowards(skinLineRenderer.GetPosition(linePointEnd), Platfrom2,
                        Time.deltaTime * speedMoveLine));

                for (int i = skinLineRenderer.positionCount - 2; i > 0; i--)
                {
                    skinLineRenderer.SetPosition(i,
                        Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i + 1),
                            Time.deltaTime * skinLineRenderer.positionCount * speedLerp));
                    speedLerp /= koef;
                }

                speedLerp = defaultSpeedLerp;
            }

            step3 = true;
            speedLerp = defaultSpeedLerp;
            while (step3 && Mathf.Abs(skinLineRenderer.GetPosition(0).y - Platfrom2.y) > 0.1f ||
                   step3 && Mathf.Abs(skinLineRenderer.GetPosition(0).x - Platfrom2.x) > 0.1f)
            {
                yield return new WaitForFixedUpdate();
                for (int i = skinLineRenderer.positionCount - 2; i > -1; i--)
                {
                    skinLineRenderer.SetPosition(i,
                        Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i + 1),
                            Time.deltaTime * skinLineRenderer.positionCount * speedLerp));
                }

            }

            step4 = true;

            currentPlatform = GameSpring.Instance.LevelGenerator.GetNextPlatform(currentPlatform);
            gameObject.transform.position = currentPlatform.PositionPlayer.position;
            gameObject.transform.SetParent(currentPlatform.PositionPlayer);
            SizePlayer(0.02f);
            HightPlayer = 0.02f;
            yield return new WaitForFixedUpdate();
            /*   while (step4 && Mathf.Abs(skinLineRenderer.GetPosition(linePointEnd).y - (Platfrom2.y + 1f)) > 0.1f)
               {
                   yield return new WaitForFixedUpdate();
                   skinLineRenderer.SetPosition(linePointEnd, Vector3.MoveTowards(skinLineRenderer.GetPosition(linePointEnd), Platfrom2 + Vector3.up * 2, Time.deltaTime * speedMoveLine));
                   SizePlayer(skinLineRenderer.GetPosition(linePointEnd).y);             
               }
               */
            if (ChangePlaftorm != null)
                ChangePlaftorm(currentPlatform);

            CanStretch();
        }
    }
}


