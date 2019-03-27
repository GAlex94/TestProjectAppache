using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TestProjectAppache
{
    public class BackGround : MonoBehaviour, IPoolObject
    {
        public bool IsPooledObject { get; set; }
        public void Deactivate()
        {
        }

        public void Activate(GameObject templatePrefab)
        {
        }
    }
}

