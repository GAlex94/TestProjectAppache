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

        public void StartGame()
        {
            Debug.Log("Start GameSpring...");
            levelGenerator.Init(objectsPool);
        }
    }
}
