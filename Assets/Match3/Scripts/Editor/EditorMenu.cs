using UnityEditor;
using Match3.Scripts.Core;
using UnityEditor.SceneManagement;

namespace Match3.Scripts.Editor
{
    [InitializeOnLoad]  public static class EditorMenu
    {
        

        [MenuItem("Match 3/Scenes/Home scene")]
        public static void OpenStartScene()
        {
            if (EditorSceneManager.GetActiveScene().name != LinkKeeper.HomeScene) 
            {
                EditorSceneManager.OpenScene($"{LinkKeeper.path}/{LinkKeeper.HomeScene}");
                DEBUGGER.DebugStr = $"Home Scene has been opened.";
                DEBUGGER.Log(ColorType.Action, DEBUGGER.DebugStr);
            }
        }

        [MenuItem("Match 3/Scenes/Level scene")]
        public static void OpenLevelScene()
        {
            if (EditorSceneManager.GetActiveScene().name != LinkKeeper.LevelScene) 
            {
                EditorSceneManager.OpenScene($"{LinkKeeper.path}/{LinkKeeper.LevelScene}");
                DEBUGGER.DebugStr = $"Level Scene has been opened.";
                DEBUGGER.Log(ColorType.Action, DEBUGGER.DebugStr);
            }
        }

        [MenuItem("Match 3/Scenes/Game scene")]
        public static void OpenGameScene()
        {
            if (EditorSceneManager.GetActiveScene().name != LinkKeeper.GameScene) 
            {
                EditorSceneManager.OpenScene($"{LinkKeeper.path}/{LinkKeeper.GameScene}");
                DEBUGGER.DebugStr = $"Game Scene has been opened.";
                DEBUGGER.Log(ColorType.Action, DEBUGGER.DebugStr);
            }
        }
    }
}
