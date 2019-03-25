using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestProjectAppache
{

    public class GameSpring : Singleton<GameSpring>, IGame
    {
        [SerializeField] private ObjectsPool objectsPool;
        [SerializeField] private LevelGenerator lecelGenerator;

        public void StartGame()
        {
            Debug.Log("Start GameSpring...");
            lecelGenerator.Init(objectsPool);
        }
    }
}
