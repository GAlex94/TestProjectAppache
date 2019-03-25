using System;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

namespace TestProjectAppache
{
    public class AnimatedScore : MonoBehaviour
    {

        public static float time = 0.2f;
        float currentScore = 0;
        float score = 0;

        TextMeshProUGUI textComponent;

        public float Score
        {
            get { return score; }
            set { score = value; }
        }

        void Awake()
        {
            textComponent = this.GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            Setup((int) currentScore);
        }

        public void Init(int curScore)
        {
            Setup(curScore);
            score = curScore;
            currentScore = score;
        }

        void Update()
        {
            if (currentScore < score)
            {
                if (Math.Abs(Time.timeScale) < 0.01f)
                {
                    currentScore = score;
                    Setup((int) currentScore);
                }

                currentScore += score / time * Time.deltaTime;
                currentScore = Mathf.Clamp(currentScore, 0, score);

                Setup((int) currentScore);
            }
            else if (currentScore > score)
            {
                if (Math.Abs(Time.timeScale) < 0.01f)
                {
                    currentScore = score;
                    Setup((int) currentScore);
                }

                currentScore -= score / time * Time.deltaTime;
                currentScore = Mathf.Max(currentScore, score);

                Setup((int) currentScore);
            }

        }

        void Setup(int n)
        {
            string s = string.Format("{0:## ### ###}", n);
            if (n == 0)
            {
                s = "0";
            }

            if (textComponent != null)
                textComponent.text = s.Trim();
        }

    }
}