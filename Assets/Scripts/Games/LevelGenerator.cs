using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestProjectAppache
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private List<Platform> platformsListPrefabs;
        [SerializeField] private List<Platform> currentPath;
        [SerializeField] private Transform spawnContainer;
        [SerializeField] private Transform StartSpawnPosition;
        [SerializeField] private List<StartPlaformPostion> startPlaformList;
        [SerializeField] private int globalPlatformIndex;
        private ObjectsPool _pool;

        
        public void Init(ObjectsPool pool)
        {
            _pool = pool;
            foreach (var obj in platformsListPrefabs)
            {
                _pool.AddObjectToPoolByType(obj.gameObject, PoolType.Tile);
            }

            GenerateFirstPlafrom();

            GameSpring.Instance.PlayerController.Init(currentPath[0]);
        }

        private void GenerateFirstPlafrom()
        {
            currentPath = new List<Platform>();
            globalPlatformIndex = 0;
            for (int i = 0; i < startPlaformList.Count; i++)
            {
                var selectType = startPlaformList[i].TypePlatform;
                var createdRailway = UseObject(StartSpawnPosition.position + startPlaformList[i].OffsetPositon, Quaternion.identity, selectType);
                StartSpawnPosition.position += startPlaformList[i].OffsetPositon;
                currentPath.Add(createdRailway);
                createdRailway.gameObject.transform.SetParent(spawnContainer);
                globalPlatformIndex++;
                createdRailway.gameObject.name = "PlatformType_"+ selectType + "_" + i;
            }

            LinkPathInGlobalPlatform();
        }

        private void LinkPathInGlobalPlatform()
        {
            for (int i = 1; i < currentPath.Count - 1; i++)
            {
                var platform = currentPath[i];
                platform.NextPlatform = currentPath[i + 1];
                platform.PrevPlatform = currentPath[i - 1];
            }

            var railway = currentPath[0];
            railway.NextPlatform = railway.PrevPlatform = currentPath[1];
            var lastPlatform = currentPath[currentPath.Count - 1];
            lastPlatform.NextPlatform = lastPlatform.PrevPlatform = currentPath[currentPath.Count - 2];

        }

        public Platform UseObject(Vector3 position, Quaternion quaternion, TypePlatformEnum typePlatform)
        {
            var spawnedTile = _pool.InstantiateFromPool(GetObject(typePlatform).gameObject, position, quaternion, PoolType.Tile);
            if (!spawnedTile.activeSelf)
            {
                spawnedTile.SetActive(true);
            }

            return spawnedTile.GetComponent<Platform>();
        }

        private Platform GetObject(TypePlatformEnum typePlatform)
        {
            var index = platformsListPrefabs.FindIndex(pl => pl.TypePlatform == typePlatform);
            return index != -1 ? platformsListPrefabs[index] : null;
        }

        public Platform GetNextPlatform(Platform currentPlatform)
        {
            var index = currentPath.FindIndex(pl => pl == currentPlatform);
            if (index != -1)
            {
                Platform nextPlatfrom = currentPath[index].NextPlatform;
                return nextPlatfrom != null ? nextPlatfrom : GenerateNewPlatform();
            }
            else
            {
                return GenerateNewPlatform();
            }
        }

        private Platform GenerateNewPlatform()
        {
            return new Platform();
        }

    }



    [Serializable]
    public class StartPlaformPostion
    {
        public TypePlatformEnum TypePlatform;
        public Vector3 OffsetPositon ;
    }
}
