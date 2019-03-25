using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TestProjectAppache
{
    [Serializable]
    public class PlayerData
    {
        public int Money;

        public string Name;

        PlayerData()
        {
            Money = 0;
            Name = "";
        }
    }

    [Serializable]
    public class GameData
    {
        public string Version;
        public float SoundValue;
        public PlayerData PlayerData;

        public GameData()
        {
            Version = "1.0";
        }
    }
}