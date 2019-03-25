using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestProjectAppache
{
    [RequireComponent(typeof(AnimatedScore))]
    public class MoneyListener : MonoBehaviour, IMoneyListener
    {
        private AnimatedScore _animScores;

        private void Awake()
        {
            _animScores = GetComponent<AnimatedScore>();
        }

        private void OnEnable()
        {
            StartCoroutine(DefferAddListber());
        }

        private void OnDisable()
        {
            DataManager.Instance.RemoveMoneyListener(this);
        }

        private IEnumerator DefferAddListber()
        {
            yield return new WaitForEndOfFrame();
            DataManager.Instance.AddMoneyListener(this);
        }

        public void OnMoneyChange(int newMoney, int oldMoney)
        {
            if (_animScores != null)
            {
                _animScores.Score = newMoney;
            }
        }
    }
}

