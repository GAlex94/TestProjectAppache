using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestProjectAppache
{
    public class Platform : MonoBehaviour, IPoolObject
    {
        public TypePlatformEnum TypePlatform;
        public Platform NextPlatform { get; set; }
        public Platform PrevPlatform { get; set; }

        public Transform PositionPlayer;

        public bool IsPooledObject { get; set; }
        public void Deactivate()
        {
        }

        public void Activate(GameObject templatePrefab)
        {
        }
    }

    public enum TypePlatformEnum
    {
        Default,
        VerticalMove,
        Horizontal,
    }
}
