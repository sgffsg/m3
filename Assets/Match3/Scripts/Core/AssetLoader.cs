using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace Match3.Scripts.Core
{
    public class AssetLoader : MonoBehaviour
    {
        public GameObject CellPrefab;
        public GameObject ChipPrefab;
        public ChipGraphics chipGraphics;
        public CellGraphics cellGraphics;
        public LevelsDB levelsDB;
        private static AssetLoader _instance;


        public static AssetLoader Instance
        {
            get
            {
                return _instance;
            }
        }

        private void Awake() 
        {
            if(_instance == null)
            {
                _instance = this;
            }

#if UNITY_EDITOR
            CellPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(LinkKeeper.pathToCellPrefab, typeof(GameObject));
            ChipPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(LinkKeeper.pathToChipPrefab, typeof(GameObject));

            chipGraphics = (ChipGraphics)AssetDatabase.LoadAssetAtPath<ChipGraphics>("Assets/Match3/Resources/ChipGraphics.asset");
            cellGraphics = (CellGraphics)AssetDatabase.LoadAssetAtPath<CellGraphics>("Assets/Match3/Resources/CellGraphics.asset");
            levelsDB = (LevelsDB)AssetDatabase.LoadAssetAtPath<LevelsDB>("Assets/Match3/Resources/LevelsDataBase.asset");
#endif
        }
    }
}
