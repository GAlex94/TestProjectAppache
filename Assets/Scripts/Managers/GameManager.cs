using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TestProjectAppache
{
    public class GameManager : Singleton<GameManager>
    {
        private bool isDebug = true;
        public bool IsDebug => isDebug;


        void Awake()
        {
            Debug.Log("GameManager awake");
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            Debug.Log("Game manager start!");

            StartCoroutine(DefferGameStart());
        }

        IEnumerator DefferGameStart()
        {
            yield return new WaitForEndOfFrame();
            
            IGame mainGameObject = FindObjectsOfType<MonoBehaviour>().OfType<IGame>().FirstOrDefault();
            if (mainGameObject != null)
                mainGameObject.StartGame();
            else
            {
                Debug.LogError("IGame object not found in scene! Game didn't launch..");
            }
        }

        public void Init(bool isDebug)
        {
            this.isDebug = isDebug;
            if (!Debug.isDebugBuild)
            {
                this.isDebug = false;
            }
        }

        #region Activators from loading scene
        public void RestoreMenu()
        {
            StartCoroutine(DefferRestoreMenu());
        }

        IEnumerator DefferRestoreMenu()
        {
            yield return null;
            Debug.Log("Menu restored");
            GUIController.Instance.ShowScreen<ScreenStartGame>();
        }

        public void ActivateSimpleGameOnScene()
        {
            StartCoroutine(DefferActivateGameOnSceneGame());
        }

        IEnumerator DefferActivateGameOnSceneGame()
        {
            yield return null;

            Debug.Log("Activate simple game...");

            IGame mainGameObject = FindObjectsOfType<MonoBehaviour>().OfType<IGame>().FirstOrDefault();
            if (mainGameObject != null)
                mainGameObject.StartGame();
            else
            {
                Debug.LogError("IGame object not found in scene! Game didn't launch..");
            }
        }
        #endregion

        public void LoadSimpleGame(string gameTag)
        {
            Debug.Log("Start load simple game");        
            SceneManager.LoadScene("loading");
        }

        public void LoadMenu()
        {
            Debug.Log("Start load menu");
            SceneManager.LoadScene("loading");
        }
    }
}
