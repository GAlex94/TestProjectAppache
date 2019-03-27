using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestProjectAppache
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("Level")]
        [SerializeField] private Level level;
        [Header("Prefabs")]
        [SerializeField] private List<Platform> platformsListPrefabs;
        [SerializeField] private GameObject backGroundPrefabs;
        [Header("Current")]
        [SerializeField] private List<Platform> currentPath;
        [SerializeField] private List<GameObject> currentBackGround;
        [Header("Spawn Information")]
        [SerializeField] private Transform spawnContainer;
        [SerializeField] private Transform StartSpawnPosition;
        private Vector3 spawnPosition;
        [SerializeField] private List<StartPlaformPostion> startPlaformList;
        [SerializeField] private int countStartSecondPlatform;
        private int globalPlatformIndex;
        [SerializeField] private float minOffsetPlatfrom;
        [SerializeField] private float maxOffsetPlatform;
        [SerializeField] private float offsetSpawnBack = 30f;
      
        private ObjectsPool _pool;


        public void Init(ObjectsPool pool)
        {
            _pool = pool;
   /*         foreach (var obj in platformsListPrefabs)
            {
                _pool.AddObjectToPoolByType(obj.gameObject, PoolType.Tile);
            }

            _pool.AddObjectToPoolByType(backGroundPrefabs, PoolType.Tile);
     */     

            GenerateFirstPlafrom();
            level.Init(StartSpawnPosition);

            GameSpring.Instance.PlayerController.Init(currentPath[0]);
            GameSpring.Instance.PlayerController.ChangePlaftorm += MoveLevel;
        }

        private void GenerateFirstPlafrom()
        {
            currentPath = new List<Platform>();
            globalPlatformIndex = 0;
            spawnPosition = StartSpawnPosition.position;
            for (int i = 0; i < startPlaformList.Count; i++)
            {
                var selectType = startPlaformList[i].TypePlatform;
                var platform = UseObject(spawnPosition + startPlaformList[i].OffsetPositon, Quaternion.identity,
                    selectType);
                spawnPosition += startPlaformList[i].OffsetPositon;
                currentPath.Add(platform);
                platform.gameObject.transform.SetParent(spawnContainer);
                globalPlatformIndex++;
                platform.gameObject.name = "PlatformType_" + selectType + "_" + i;
            }

            for (int i = 0; i < countStartSecondPlatform; i++)
            {
                CreateNewPlatform();
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
            var spawnedTile =
                _pool.InstantiateFromPool(GetObject(typePlatform).gameObject, position, quaternion, PoolType.Tile);
            if (!spawnedTile.activeSelf)
            {
                spawnedTile.SetActive(true);
            }

            return spawnedTile.GetComponent<Platform>();
        }

         public GameObject UseObject(Vector3 position, Quaternion quaternion)
        {
            var spawnedTile =
                _pool.InstantiateFromPool(backGroundPrefabs, position, quaternion, PoolType.Tile);
            if (!spawnedTile.activeSelf)
            {
                spawnedTile.SetActive(true);
            }

            return spawnedTile;
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
                if (nextPlatfrom != null)
                {
                    return nextPlatfrom;
                }else
                {
                    CreateNewPlatform();
                    return currentPath.Last();
                }          
            }
            else
            {
                CreateNewPlatform();
                return currentPath.Last();
            }
        }

        private void CreateNewPlatform()
        {
            HideFirstPlatform();
            var lastPlatform = currentPath[currentPath.Count - 1];
            var selectTypePlatform = globalPlatformIndex % 5 != 0 ? TypePlatformEnum.Default : (UnityEngine.Random.Range(0,100) > 50 ? TypePlatformEnum.VerticalMove : TypePlatformEnum.Horizontal);
            var offset = UnityEngine.Random.Range(minOffsetPlatfrom, maxOffsetPlatform);
            spawnPosition = lastPlatform.transform.position + Vector3.right * offset;
            var platform = UseObject(spawnPosition, Quaternion.identity, selectTypePlatform);
            currentPath.Add(platform);
            platform.gameObject.transform.SetParent(spawnContainer);
            globalPlatformIndex++;
            platform.gameObject.name = "PlatformType_" + selectTypePlatform + "_" + globalPlatformIndex;

            lastPlatform.NextPlatform = platform;
            platform.PrevPlatform = lastPlatform;
            platform.NextPlatform = null;
            globalPlatformIndex++;
        }

        public void HideFirstPlatform()
        {
            if (currentPath.Count < 10)
            {
                return;
            }
            var destroyObj = currentPath[0].gameObject;
            currentPath.RemoveAt(0);
            _pool.DestroyFromPool(destroyObj);
        }

 

        public void MoveLevel(Platform currentPlatform)
        {
            level.Move(currentPlatform.gameObject.transform);
            CreateNewPlatform();
        }

        [ContextMenu("GenerateNewBack")]
        public void GenerateNewBack()
        {
            HideFirstBack();
            var spawnPositionBack = currentBackGround.Last().transform.position + Vector3.right * offsetSpawnBack;
            var newBack = UseObject(spawnPositionBack, Quaternion.identity);
            newBack.transform.SetParent(level.gameObject.transform);
            currentBackGround.Add(newBack);        
        }

        public void HideFirstBack()
        {
            if (currentBackGround.Count < 2)
            {
                return;
            }
            var destroyObj = currentBackGround[0].gameObject;
            currentBackGround.RemoveAt(0);
            _pool.DestroyFromPool(destroyObj);
        }
    }



    [Serializable]
    public class StartPlaformPostion
    {
        public TypePlatformEnum TypePlatform;
        public Vector3 OffsetPositon ;
    }
}
