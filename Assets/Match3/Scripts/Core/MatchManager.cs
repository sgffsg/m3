using Match3.Scripts.Save;
using System.Globalization;
using UnityEngine;
using System.IO;
using System;

namespace Match3.Scripts.Core
{
    public class MatchManager : MonoBehaviour
    {
        #region SingleTone
        private static MatchManager _instance;
        
        public static MatchManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public bool IsMobile
        {
            get
            {
                return _isMobile;
            }
        }

        private bool _isMobile;
        private int targetFrameRate = 30;
        #endregion
        public GameData gameData;
        
        private void Awake()
        {
            if (File.Exists(LinkKeeper.saveFile))
            {
                this.gameData = BinarySerializer.Deserialize<GameData>(LinkKeeper.saveFile);
                this.gameData.dateLastPlayed = Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture);
                SaveData();
            }
            else
            {
                CreatePrimaryData();
            }

            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(this);
            }
            

            _isMobile = Application.isMobilePlatform;
            Application.targetFrameRate = targetFrameRate;

            DEBUGGER.DebugStr = $"Application is started on Platform: {Application.platform} with FrameRate: {Application.targetFrameRate}.";
            DEBUGGER.Log(ColorType.System, DEBUGGER.DebugStr);
            DontDestroyOnLoad(this);
        }


        public void CreatePrimaryData()
        {
            this.gameData.currentLevel = 1;
            this.gameData.dateLastPlayed = Convert.ToString(DateTime.Now, CultureInfo.InvariantCulture);
            this.gameData.lifesCount = 5;
            this.gameData.maxLifesCount = 5;
            SaveData();
        }

        [ContextMenu(itemName:"Save")]
        public void SaveData()
        {
            BinarySerializer.Serialize(LinkKeeper.saveFile, this.gameData);
        }

        [ContextMenu(itemName:"Reset")]
        public void ResetData()
        {
            if (File.Exists(LinkKeeper.saveFile))
            {
                File.Delete(LinkKeeper.saveFile);
                CreatePrimaryData();
            }
        }
    }

    
}
