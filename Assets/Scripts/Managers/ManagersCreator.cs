using UnityEngine;

namespace TestProjectAppache
{
    public class ManagersCreator : MonoBehaviour
    {
        [Header("Game managers settings")]
        [SerializeField]
        private bool isDebug = true;

        [Header("Data managers settings")]
        [SerializeField]
        private string profileName = "MainProfile";

        [SerializeField]
        private bool clearProfile = false;

        [SerializeField]
        private DefaultProfile defaultProfile = null;

        void Awake()
        {
            Debug.Log("ManagersCreator awake");
            if (!GameManager.IsAwake)
            {
                DataManager.Instance.Init(profileName, clearProfile, defaultProfile);
                GameManager.Instance.Init(isDebug);
            }
        }
    }
}
