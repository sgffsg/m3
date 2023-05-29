using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Match3.Scripts.Core
{
    public static class LinkKeeper
    {
        public static string saveFile = Path.Combine(Application.persistentDataPath, "game.data");
        public static string pathToChipPrefab = "Assets/Match3/Prefabs/Chip Prefab.prefab";
        public static string pathToCellPrefab = "Assets/Match3/Prefabs/Cell Prefab.prefab";
        public static string path = "Assets/Match3/Scenes";
        public static string HomeScene = "HomeScene.unity";
        public static string LevelScene = "LevelScene.unity";
        public static string GameScene = "GameScene.unity";
        public static int currentLevel = 1;
    }
}
