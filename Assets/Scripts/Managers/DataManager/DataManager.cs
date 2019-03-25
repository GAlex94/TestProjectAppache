
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace TestProjectAppache
{
    public class DataManager : Singleton<DataManager>
    {
        private string profileName;
        private bool clearProfileOnStart;
        [SerializeField]  private GameData data = new GameData();
        private bool dataDirty = false;

        private DefaultProfile defaultProfile;
      
        private List<IMoneyListener> moneyListeners = new List<IMoneyListener>();

        public void SetSoundValue(float soundValue, bool autoSave = true)
        {
            data.SoundValue = soundValue;
            if(autoSave)
                Save();
        }

        public float SoundValue
        {
            get { return data.SoundValue; }
        }

        private string FilePath
        {
            get { return Path.Combine(Application.persistentDataPath, profileName + ".json"); }
        }

        public GameData GetCurrentData
        {
            get { return data; } 
        }

        void Awake()
        {
            Debug.Log("DataManager awake");
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            Debug.Log("DataManager start");
            if (clearProfileOnStart)
            {
                Clear();
            }
            else
            {
               Load();
            }
        }

        public void Init(string profileName, bool clearProfileOnStart, DefaultProfile defaultProfile)
        {
            this.profileName = profileName;
            this.clearProfileOnStart = clearProfileOnStart;
            this.defaultProfile = defaultProfile;

            if (!Debug.isDebugBuild)
                this.clearProfileOnStart = false;

            Debug.Log("Profile path: " + FilePath);
        }


        public void Clear()
        {
            Debug.Log("CLEAR");
            data = defaultProfile != null ? defaultProfile.profileData : new GameData();

            Save();

            if (File.Exists(FilePath))
            {
                Load();
            }
            else
            {
                Debug.LogError("Profile not saved! Check file system!");
                data = new GameData();
            }

            SetSoundValue(data.SoundValue, false);
        }

        [ContextMenu("Save")]
        public void Save()
        {
            Debug.Log("SAVE");
            File.WriteAllText(FilePath, JsonUtility.ToJson(data, false));
        }

        void Load()
        {
            if (File.Exists(FilePath))
            {
                Debug.Log("LOAD");
                data = JsonUtility.FromJson<GameData>(File.ReadAllText(FilePath));

                UpdateRuntimeByLoadedData();
            }
            else
            {
                Clear();
            }
        }

        private void SetDataDirty()
        {
            if (dataDirty == false)
            {
                dataDirty = true;
                Invoke("DefferSave", 1.0f);
            }
        }

        void DefferSave()
        {
            Save();
            dataDirty = false;
        }

        void UpdateRuntimeByLoadedData()
        {                
            SetSoundValue(data.SoundValue, false);
        }

        #region Money

        public int Money
        {
            get { return data.PlayerData.Money; }
        }

#if UNITY_EDITOR
        public void SetMoney(int money)
        {
            int lastMoney = data.PlayerData.Money;

            data.PlayerData.Money = money;

            if (moneyListeners.Count > 0)
            {
                moneyListeners.ForEach(curListener => curListener.OnMoneyChange(data.PlayerData.Money, lastMoney));
            }
            SetDataDirty();
        }
#endif

        public void AddMoney(int money)
        {
            int lastMoney = data.PlayerData.Money;
            data.PlayerData.Money += Mathf.Max(money, 0);

            if (lastMoney != data.PlayerData.Money && moneyListeners.Count > 0)
            {
                moneyListeners.ForEach(curListener => curListener.OnMoneyChange(data.PlayerData.Money, lastMoney));
            }
            SetDataDirty();
        }

        public bool CheckForSpend(int money)
        {
            return money <= data.PlayerData.Money;
        }

        public int SpendMoney(int money)
        {
            if (CheckForSpend(money))
            {
                int lastMoney = data.PlayerData.Money;
                data.PlayerData.Money = Mathf.Max(data.PlayerData.Money - Mathf.Max(money, 0), 0);

                if (lastMoney != data.PlayerData.Money && moneyListeners.Count > 0)
                {
                    moneyListeners.ForEach(curListener => curListener.OnMoneyChange(data.PlayerData.Money, lastMoney));
                }
                SetDataDirty();
            }

            return data.PlayerData.Money;
        }

        public void AddMoneyListener(IMoneyListener listener)
        {
            if (!moneyListeners.Contains(listener))
                moneyListeners.Add(listener);
        }

        public void RemoveMoneyListener(IMoneyListener listener)
        {
            moneyListeners.Remove(listener);
        }

        #endregion

        #region PlayerName
        public string PlayerName
        {
            get { return data.PlayerData.Name; }
        }

        public void SetNewPlayerName(string value)
        {
            data.PlayerData.Name = value;
            SetDataDirty();
        }
        #endregion

    }
}
