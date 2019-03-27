using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestProjectAppache
{

    public class GameSpring : Singleton<GameSpring>, IGame
    {
        [SerializeField] private ObjectsPool objectsPool;
        [SerializeField] private LevelGenerator levelGenerator;
        [SerializeField] private PlayerController playerController;

        public PlayerController PlayerController => playerController;
        public LevelGenerator LevelGenerator => levelGenerator;

        public int CurrentScore { get => currentScore;}
        public Action<int> ChangeScore;
        private int currentScore;

        public void StartGame()
        {
            Debug.Log("Start GameSpring...");
            levelGenerator.Init(objectsPool);

            currentScore = 0;
        }

        public void AddScore(int countScore = 1)
        {
            currentScore += Mathf.Max(countScore, 0);
            DataManager.Instance.AddMoney(countScore);
            if (ChangeScore != null)
            {
                ChangeScore(CurrentScore);
            }
        }

        [ContextMenu("GameOver")]
        public void GameOver()
        {
            GUIController.Instance.ShowScreen<ScreenGameOver>();
        }

    }
}
