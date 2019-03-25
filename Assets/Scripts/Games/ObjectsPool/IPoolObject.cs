using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestProjectAppache
{
    public interface IPoolObject
    {
        bool IsPooledObject { get; set; }
        void Deactivate();
        void Activate(GameObject templatePrefab);
    }
}
