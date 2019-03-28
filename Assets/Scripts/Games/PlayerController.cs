using System;
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
        [SerializeField] private float OffesetMove = 0.2f;

        [SerializeField] private float HightPlayer;
        [SerializeField] private float speed = 1;
        [SerializeField] private float speedMoveLine = 1;
        [SerializeField] private float speedLerp = 1;
        [SerializeField] private float defaultSpeedLerp = 1;

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
            var step = needPosition / (skinLineRenderer.positionCount - 1);
            var position = Vector3.zero;
            for (int i = 0; i < skinLineRenderer.positionCount; i++)
            {
                skinLineRenderer.SetPosition(i, position);
                position += Vector3.up * step;
            }
        }


        public void MoveToPlatform()
        {
            Debug.Log("Move to next platform");
            var nextPlatfrom = GameSpring.Instance.LevelGenerator.GetNextPlatform(currentPlatform);
            if (Math.Abs(Math.Abs(nextPlatfrom.PositionPlayer.position.x - currentPlatform.PositionPlayer.position.x) -
                         HightPlayer) < OffesetMove)
            {
                GameSpring.Instance.AddScore(currentPlatform.Reward);
                MovePlayer(currentPlatform, nextPlatfrom);
            }
            else
            {
                if (GameManager.Instance.IsDebug)
                {
                    //     HightPlayer = 1;
                    //     SizePlayer(HightPlayer);
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


        [ContextMenu("Set Size")]
        public void TEst()
        {
            var nextPlatfrom = GameSpring.Instance.LevelGenerator.GetNextPlatform(currentPlatform);
            HightPlayer = Math.Abs(nextPlatfrom.PositionPlayer.position.x - currentPlatform.PositionPlayer.position.x);
            SizePlayer(HightPlayer * KoefHight);

            var target = transform.InverseTransformPoint(nextPlatfrom.PositionPlayer.position);
            maxPost = skinLineRenderer.positionCount - 1;
            radius = Math.Abs(Math.Abs(skinLineRenderer.GetPosition(maxPost).x - target.x) / 2);
        }

        [ContextMenu("Move")]
        public void TEst2()
        {
            var nextPlatfrom = GameSpring.Instance.LevelGenerator.GetNextPlatform(currentPlatform);
            MovePlayer(currentPlatform, nextPlatfrom);
        }

        private void MovePlayer(Platform currentPlatform, Platform nextPlatfrom)
        {
            StartCoroutine(Moving(nextPlatfrom));
/*            currentPlatform = nextPlatfrom;
            gameObject.transform.position = currentPlatform.PositionPlayer.position;
            gameObject.transform.SetParent(currentPlatform.PositionPlayer);
            HightPlayer = 1;
            SizePlayer(HightPlayer);
            if (ChangePlaftorm != null) ChangePlaftorm(currentPlatform);*/
        }

        public bool isCircle;
        public float alpha = 0f;
        public float radius;
        public int maxPost;
        public float Teas = 0.5f;
        private void Update()
        {
            if (isCircle)
            {
                alpha += Time.deltaTime;
                var x = radius * Mathf.Cos(alpha * speed); // + a;
                var y = radius * Mathf.Sin(alpha * speed); // + b;
                transform.position = new Vector2(x, y);

            }
        }


        private int linePointEnd;
        private int linePointCenter;
        public Vector3 center;
        public Vector3 startPos;
        public Vector3 endPos;
        public float Radius;
        public bool step0;
        public bool step1;
        public bool step2;
        public bool step3;
        public bool step4;
        public bool step5;
        public bool step6;
        public bool step7;
        public float alfa = 0;
        public float koef = 1f;


        IEnumerator Moving(Platform nextPlatfrom)
        {
            yield return null;
            linePointEnd = skinLineRenderer.positionCount - 1;
            linePointCenter = skinLineRenderer.positionCount > 3 ? (skinLineRenderer.positionCount / 2) : 0;
            var Platfrom2 = transform.InverseTransformPoint(nextPlatfrom.PositionPlayer.position);
            SizePlayer(Math.Abs(skinLineRenderer.GetPosition(0).x - Platfrom2.x));
            endPos = Platfrom2;
            var d = (Platfrom2 - skinLineRenderer.GetPosition(linePointEnd));
          center = skinLineRenderer.GetPosition(linePointEnd) + d / 2;
          Radius = d.magnitude / 2;
          Debug.Log((skinLineRenderer.GetPosition(linePointEnd).x - center.x) / Radius);
          alfa = Mathf.Asin((skinLineRenderer.GetPosition(linePointEnd).x - center.x) / Radius);
          startPos = skinLineRenderer.GetPosition(linePointEnd);

            step0 = true;
            yield return null;

        /*    while (step0 && Mathf.Abs(skinLineRenderer.GetPosition(linePointEnd).y - (startPos.y - 0.5f)) > 0.1f)
            {
                yield return new WaitForFixedUpdate();
                skinLineRenderer.SetPosition(linePointEnd, Vector3.MoveTowards(skinLineRenderer.GetPosition(linePointEnd), startPos - Vector3.up*0.5f, Time.deltaTime * speedMoveLine));
            }

            while (step0 && Mathf.Abs(skinLineRenderer.GetPosition(linePointEnd).y - startPos.y) > 0.1f)
            {
                yield return new WaitForFixedUpdate();
                skinLineRenderer.SetPosition(linePointEnd, Vector3.MoveTowards(skinLineRenderer.GetPosition(linePointEnd), startPos, Time.deltaTime * speedMoveLine));
            }
            */

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
                    skinLineRenderer.SetPosition(i, Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i + 1), Time.deltaTime * skinLineRenderer.positionCount * speedLerp));
                    speedLerp /= koef;
                }

                speedLerp = defaultSpeedLerp;
            }

        
            step2 = true;

            while (step2 && Mathf.Abs(skinLineRenderer.GetPosition(linePointEnd).y - Platfrom2.y) > 0.1f)
            {
                yield return new WaitForFixedUpdate();
                skinLineRenderer.SetPosition(linePointEnd, Vector3.MoveTowards(skinLineRenderer.GetPosition(linePointEnd), Platfrom2, Time.deltaTime * speedMoveLine));

                for (int i = skinLineRenderer.positionCount - 2; i > 0; i--)
                {
                    skinLineRenderer.SetPosition(i, Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i + 1), Time.deltaTime * skinLineRenderer.positionCount * speedLerp));
                    speedLerp /= koef;
                }

                speedLerp = defaultSpeedLerp;
            }

            step3 = true;
            speedLerp = defaultSpeedLerp;
            while (step3 && Mathf.Abs(skinLineRenderer.GetPosition(0).y - Platfrom2.y) > 0.1f  || step3 && Mathf.Abs(skinLineRenderer.GetPosition(0).x - Platfrom2.x) > 0.1f)
            {
                yield return new WaitForFixedUpdate();
                for (int i = skinLineRenderer.positionCount - 2; i > -1; i--)
                {
                    skinLineRenderer.SetPosition(i, Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i + 1), Time.deltaTime * skinLineRenderer.positionCount * speedLerp));
                }

            }

            step4 = true;

            currentPlatform = nextPlatfrom;
            gameObject.transform.position = currentPlatform.PositionPlayer.position;
            gameObject.transform.SetParent(currentPlatform.PositionPlayer);
            SizePlayer(0.02f);
            //  Platfrom2 = currentPlatform.PositionPlayer.transform.position;
            /*
            for (int i = 0; i < skinLineRenderer.positionCount - 1; i++)
            {
               skinLineRenderer.SetPosition(i, Platfrom2);
            }*/

            yield return new WaitForFixedUpdate();
            while (step4 && Mathf.Abs(skinLineRenderer.GetPosition(linePointEnd).y - (Platfrom2.y + 1f)) > 0.1f)
            {
                yield return new WaitForFixedUpdate();
                skinLineRenderer.SetPosition(linePointEnd, Vector3.MoveTowards(skinLineRenderer.GetPosition(linePointEnd), Platfrom2 + Vector3.up * 2, Time.deltaTime * speedMoveLine));
                SizePlayer(skinLineRenderer.GetPosition(linePointEnd).y);             
            }

            if (ChangePlaftorm != null) ChangePlaftorm(currentPlatform);

            //  while (step3 && Mathf.Abs(skinLineRenderer.GetPosition(0).y - startPos.y) > 0.1f)
            //  {
            //      yield return new WaitForFixedUpdate();
            ////      skinLineRenderer.SetPosition(0, Vector3.MoveTowards(skinLineRenderer.GetPosition(0), startPos, Time.deltaTime * speedMoveLine));
            //      for (int i = skinLineRenderer.positionCount - 2; i > -1; i--)
            //      {
            //          skinLineRenderer.SetPosition(i, Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i + 1), Time.deltaTime * skinLineRenderer.positionCount * speedLerp));
            //      //    speedLerp /= koef;
            //      }
            //      /*
            //      for (int i = 1; i > skinLineRenderer.positionCount - 2; i--)
            //      {
            //          skinLineRenderer.SetPosition(i, Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i - 1), Time.deltaTime * skinLineRenderer.positionCount  *  speedLerp));
            //      }*/

            //  }



            //            step4 = true;

            //            while (step4)
            //            {
            //               // yield return new WaitForFixedUpdate();
            //                alfa = Mathf.Asin((skinLineRenderer.GetPosition(0).x - center.x) / Radius);
            //                step4 = false;

            //            }

            //           step5 = true;
            //            while (step5 && Mathf.Abs(skinLineRenderer.GetPosition(0).x - Platfrom2.x) > 0.1f)
            //            {
            //                yield return new WaitForFixedUpdate();
            //                alfa += Time.deltaTime * speed;
            //                var x = Radius * Mathf.Sin(alfa);
            //                var y = Radius * Mathf.Cos(alfa);
            //                skinLineRenderer.SetPosition(0, new Vector3(x, y) + center);

            //              /*  for (int i = skinLineRenderer.positionCount - 2; i > 0; i--)
            //                {
            //                    skinLineRenderer.SetPosition(i, Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i + 1), Time.deltaTime * speedLerp));
            //                }*/

            //                /*   for (int i = skinLineRenderer.positionCount - 2; i > 0; i--)
            //                   {
            //                       skinLineRenderer.SetPosition(i, Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i + 1), Time.deltaTime * speedLerp));
            //                   }*/
            //            }
            //            step6 = true;
            //            while (step6 && Mathf.Abs(skinLineRenderer.GetPosition(0).y - (Platfrom2.y + 0.5f))  > 0.1f)
            //            {
            //                yield return new WaitForFixedUpdate();
            //                skinLineRenderer.SetPosition(0,Vector3.MoveTowards(skinLineRenderer.GetPosition(0), Platfrom2 + Vector3.up * 0.5f,Time.deltaTime * speedMoveLine));


            //            }

            //            step7 = true;
            //            while (step7 && Mathf.Abs(skinLineRenderer.GetPosition(0).y - (Platfrom2.y + 1f)) > 0.1f)
            //            {
            //                yield return new WaitForFixedUpdate();
            //                skinLineRenderer.SetPosition(0, Vector3.MoveTowards(skinLineRenderer.GetPosition(0), Platfrom2 + Vector3.up * 1f, Time.deltaTime * speedMoveLine));


            //            }












            /*Debug.Log(target);
            var maxPost = skinLineRenderer.positionCount - 1;
            bool startBakcMove = false;
            var heading = target - skinLineRenderer.GetPosition(maxPost);
            radius = heading.magnitude / 3; //Math.Abs(Math.Abs(skinLineRenderer.GetPosition(maxPost).x - target.x) / 2);
                
            var radius2 = radius * 2;
            var a = (target - skinLineRenderer.GetPosition(0)).magnitude / 2 ;
            var b = a/2 ;
            var alpha = 0f;
            // меняется плавно значение угла

              var testCount = 0;

            //Первоначальное движение
            while ((target - skinLineRenderer.GetPosition(maxPost)).magnitude > Teas && testCount < 100000000)
            {
                Debug.LogError((target - skinLineRenderer.GetPosition(maxPost)).magnitude);
                yield return new WaitForFixedUpdate();
                var x = radius * Mathf.Cos(alpha * speed);
                var y = radius * Mathf.Sin(alpha * speed);
                alpha += Time.deltaTime;
                x *= -1;
                testCount++;
                skinLineRenderer.SetPosition(maxPost, new Vector3(x, y) + new Vector3(a, b));

                for (int i = skinLineRenderer.positionCount - 2; i > 0; i--)
                {
                    skinLineRenderer.SetPosition(i,Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i + 1), Time.deltaTime * speedLerp));

                }
            }
            
            while (Math.Abs(skinLineRenderer.GetPosition(maxPost).y - target.y) > 0.01f ||
                   Math.Abs(skinLineRenderer.GetPosition(maxPost).x - target.x) > 0.01f)

            {
                yield return new WaitForFixedUpdate();

                skinLineRenderer.SetPosition(0, Vector3.MoveTowards(skinLineRenderer.GetPosition(0), target, Time.deltaTime * speed));
                for (int i = skinLineRenderer.positionCount - 2; i > 0; i--)
                {
                    skinLineRenderer.SetPosition(i, Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i + 1), Time.deltaTime * speedLerp));
                }
            }



            //       skinLineRenderer.SetPosition(maxPost, Vector3.MoveTowards(skinLineRenderer.GetPosition(maxPost), target, Time.deltaTime * speed));

            /*   for (int i = skinLineRenderer.positionCount - 2; i > 0; i--)
               {
                   var Yinterp = Mathf.Lerp(skinLineRenderer.GetPosition(i - 1).y,
                       skinLineRenderer.GetPosition(i + 1).y, skinLineRenderer.GetPosition(i).y);
                   var Xinterp = Mathf.Lerp(skinLineRenderer.GetPosition(i - 1).x,
                       skinLineRenderer.GetPosition(i + 1).x, skinLineRenderer.GetPosition(i).x);
                   skinLineRenderer.SetPosition(i, new Vector3(Xinterp, Yinterp));
               }*/



            //while (Math.Abs(skinLineRenderer.GetPosition(maxPost).x - target.x) > radius)
            //{
            //    yield return new WaitForFixedUpdate();
            //    skinLineRenderer.SetPosition(maxPost,
            //        Vector3.MoveTowards(skinLineRenderer.GetPosition(maxPost), Vector3.right * target.x,
            //            Time.deltaTime * speed));

            //    for (int i = skinLineRenderer.positionCount - 2; i > 0; i--)
            //    {
            //        skinLineRenderer.SetPosition(i,Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i+1), Time.deltaTime * speedLerp));

            //        /*     var Yinterp = Mathf.Lerp(skinLineRenderer.GetPosition(i - 1).y,
            //                   skinLineRenderer.GetPosition(i + 1).y, skinLineRenderer.GetPosition(i).y);
            //               var Xinterp = Mathf.Lerp(skinLineRenderer.GetPosition(i - 1).x,
            //                   skinLineRenderer.GetPosition(i + 1).x, skinLineRenderer.GetPosition(i).x);*/
            //     //              Mathf.Lerp()
            //   /*     var Yinterp = Mathf.Lerp(skinLineRenderer.GetPosition(i + 1).y,
            //            skinLineRenderer.GetPosition(i - 1).y, skinLineRenderer.GetPosition(i).y);
            //        var Xinterp = Mathf.Lerp(skinLineRenderer.GetPosition(i + 1).x,
            //            skinLineRenderer.GetPosition(i - 1).x, skinLineRenderer.GetPosition(i).x);
            //            */
            //       // skinLineRenderer.SetPosition(i, new Vector3(Xinterp, Yinterp));
            //    }
            //}

            //while (Math.Abs(skinLineRenderer.GetPosition(maxPost).y - target.y) > 0.01f ||
            //       Math.Abs(skinLineRenderer.GetPosition(maxPost).x - target.x) > 0.01f)

            //{
            //    yield return new WaitForFixedUpdate();

            //    skinLineRenderer.SetPosition(maxPost,
            //        Vector3.MoveTowards(skinLineRenderer.GetPosition(maxPost), target, Time.deltaTime * speed));

            //    for (int i = skinLineRenderer.positionCount - 2; i > 0; i--)
            //    {
            //        skinLineRenderer.SetPosition(i, Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i + 1), Time.deltaTime * speedLerp));
            //        /*    var Yinterp = Mathf.Lerp(skinLineRenderer.GetPosition(i + 1).y,
            //                skinLineRenderer.GetPosition(i - 1).y, skinLineRenderer.GetPosition(i).y);
            //            var Xinterp = Mathf.Lerp(skinLineRenderer.GetPosition(i + 1).x,
            //                skinLineRenderer.GetPosition(i - 1).x, skinLineRenderer.GetPosition(i).x);
            //            skinLineRenderer.SetPosition(i, new Vector3(Xinterp, Yinterp));*/
            //    }
            //}

            //var testCount = 0;
            //while (Math.Abs(skinLineRenderer.GetPosition(0).y - (target.y + 1)) > 0.01f && testCount < 10000000 ||
            //       Math.Abs(skinLineRenderer.GetPosition(0).x - (target.x)) > 0.01f && testCount < 10000000)
            //{
            //    testCount++;
            //    yield return new WaitForFixedUpdate();
            //    skinLineRenderer.SetPosition(0,
            //        Vector3.MoveTowards(skinLineRenderer.GetPosition(0), target + Vector3.up, Time.deltaTime * speed));
            //    for (int i = skinLineRenderer.positionCount - 2; i > 0; i--)
            //    {
            //        skinLineRenderer.SetPosition(i, Vector3.MoveTowards(skinLineRenderer.GetPosition(i), skinLineRenderer.GetPosition(i + 1), Time.deltaTime * speedLerp));
            //        /*
            //        var Yinterp = Mathf.Lerp(skinLineRenderer.GetPosition(i + 1).y,
            //            skinLineRenderer.GetPosition(i - 1).y, skinLineRenderer.GetPosition(i).y);
            //        var Xinterp = Mathf.Lerp(skinLineRenderer.GetPosition(i + 1).x,
            //            skinLineRenderer.GetPosition(i - 1).x, skinLineRenderer.GetPosition(i).x);
            //        skinLineRenderer.SetPosition(i, new Vector3(Xinterp, Yinterp));*/

            //    }
            //}



        }
    }
}


