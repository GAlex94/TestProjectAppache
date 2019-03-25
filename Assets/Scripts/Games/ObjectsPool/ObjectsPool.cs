using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestProjectAppache
{
    [Serializable]
    class WaitPoolObject
    {
        public GameObject prefab;
        public GameObject lifeObject;
        public IPoolObject poolObject;
        public bool isUsed;
        public PoolType poolType;
    }

    public enum PoolType
    {
        Tile
    }

    [Serializable]
    public class PoolTypeObjects
    {
        public PoolType poolType;
        public int countPregenerateByPrefab;
        public GameObject[] prefabsForPool;
    }

    public class ObjectsPool : MonoBehaviour
    {
        [SerializeField] private PoolTypeObjects[] poolTypes;

        [SerializeField] private bool disabledRealInstantiate;

        List<WaitPoolObject> poolObjects = new List<WaitPoolObject>();
        private int[] processIndicies = new int[100];

        void Start()
        {
            Init();
        }


        bool isInit = false;

        void Init()
        {
            if (isInit)
                return;
            isInit = true;


            bool isDisableInstantiate = disabledRealInstantiate;

            disabledRealInstantiate = false;
            foreach (var curPoolType in poolTypes)
            {
                foreach (var curPrefab in curPoolType.prefabsForPool)
                {
                    for (int i = 0; i < curPoolType.countPregenerateByPrefab; i++)
                    {
                        InstantiateFromPool(curPrefab, Vector3.zero, Quaternion.identity, curPoolType.poolType);
                    }
                }
            }

            foreach (var curPoolObject in poolObjects)
            {
                DestroyFromPool(curPoolObject.lifeObject);
            }

            disabledRealInstantiate = isDisableInstantiate;
        }

        public void AddObjectToPoolByType(GameObject prefab, PoolType poolType)
        {
            //поулчаем данные о типе добавляемого объекта
            PoolTypeObjects objects = poolTypes.FirstOrDefault(x => x.poolType == poolType);

            if (objects == null)
            {
                //   Debug.LogErrorFormat("cant find pool with type = {0}", poolType);
                return;
            }

            //добавляем элемент в массив
            GameObject[] oldPoolObjects = objects.prefabsForPool;

            objects.prefabsForPool = new GameObject[oldPoolObjects.Length + 1];
            for (int i = 0; i < objects.prefabsForPool.Length - 1; i++)
            {
                objects.prefabsForPool[i] = oldPoolObjects[i];
            }

            objects.prefabsForPool[objects.prefabsForPool.Length - 1] = prefab;

            //инициализируум

            bool isDisableInstantiate = disabledRealInstantiate;
            disabledRealInstantiate = false;

            for (int i = 0; i < objects.countPregenerateByPrefab; i++)
            {
                //спауним новые префабы
                InstantiateFromPool(prefab, Vector3.zero, Quaternion.identity, objects.poolType);
            }

            //выключаем новые префабы
            foreach (var curPoolObject in poolObjects)
            {
                if (curPoolObject.prefab == prefab)
                    DestroyFromPool(curPoolObject.lifeObject);
            }

            disabledRealInstantiate = isDisableInstantiate;
        }

        public GameObject InstantiateFromPool(GameObject prefab, Vector3 position, Quaternion rotation,
            PoolType poolType)
        {
            Init();

            int foundIndex = poolObjects.FindIndex(o => o.prefab == prefab && !o.isUsed);

            if (foundIndex == -1 && disabledRealInstantiate)
            {
                foundIndex = FindFreePoolObjectIndex(poolType);
            }

            if (foundIndex == -1)
            {
                if (disabledRealInstantiate)
                {
                    return null;
                }

                WaitPoolObject newPoolObject = InstantiateNew(prefab, poolType);
                if (newPoolObject != null)
                {
                    poolObjects.Add(newPoolObject);
                    newPoolObject.lifeObject.SetActive(true);
                    newPoolObject.isUsed = true;
                    newPoolObject.lifeObject.transform.position = position;
                    newPoolObject.lifeObject.transform.rotation = rotation;
                    newPoolObject.poolObject.Activate(newPoolObject.prefab);
                    return newPoolObject.lifeObject;
                }
                else
                {
                    Debug.LogWarning("Instantiate without pull! Object " + prefab.name +
                                     " didn't contain IPoolObject interface!");
                    GameObject newObject = Instantiate(prefab);
                    newObject.transform.position = position;
                    newObject.transform.rotation = rotation;
                    return newObject;
                }
            }
            else
            {
                WaitPoolObject newPoolObject = poolObjects[foundIndex];
                newPoolObject.lifeObject.SetActive(true);
                newPoolObject.isUsed = true;
                newPoolObject.lifeObject.transform.position = position;
                newPoolObject.lifeObject.transform.rotation = rotation;
                newPoolObject.poolObject.Activate(newPoolObject.prefab);
                Debug.Log("Exist 'instantiate' pool object. Name=" + newPoolObject.lifeObject.name);
                return newPoolObject.lifeObject;
            }
        }

        public void DestroyFromPool(GameObject currentObject)
        {
            Debug.Log("DestroyObject from pool: " + currentObject.name);
            int foundIndex = poolObjects.FindIndex(o => o.lifeObject == currentObject && o.isUsed);
            if (foundIndex != -1)
            {
                poolObjects[foundIndex].poolObject.Deactivate();
                poolObjects[foundIndex].lifeObject.SetActive(false);
                poolObjects[foundIndex].isUsed = false;
                Debug.Log("Destroy 'instantiate' pool object. Name=" + poolObjects[foundIndex].lifeObject.name);
            }
        }

        WaitPoolObject InstantiateNew(GameObject prefab, PoolType poolType)
        {
            if (prefab.GetComponent<IPoolObject>() != null)
            {
                GameObject newGameObject = Instantiate(prefab);
                newGameObject.transform.SetParent(this.transform, true);
                IPoolObject poolObject = newGameObject.GetComponent<IPoolObject>();
                poolObject.IsPooledObject = true;

                WaitPoolObject newObj = new WaitPoolObject()
                {
                    isUsed = true,
                    lifeObject = newGameObject,
                    poolObject = poolObject,
                    prefab = prefab,
                    poolType = poolType
                };
                Debug.Log("New instantiate pool object. Name=" + newGameObject.name);
                return newObj;
            }

            return null;
        }


        int FindFreePoolObjectIndex(PoolType type)
        {
            int countFree = 0;
            for (int i = 0; i < poolObjects.Count; i++)
            {
                if (poolObjects[i].poolType == type && !poolObjects[i].isUsed)
                    processIndicies[countFree++] = i;
            }

            if (countFree == 0)
                return -1;

            int randomIndex = UnityEngine.Random.Range(0, countFree);
            return processIndicies[randomIndex];
        }
    }
}
