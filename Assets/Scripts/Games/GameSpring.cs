using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestProjectAppache
{

    public class GameSpring : Singleton<GameSpring>, IGame
    {
        public void StartGame()
        {
            Debug.Log("Start GameSpring...");
        }
    }
}
