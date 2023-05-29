using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Levels;
using UnityEngine;
using UnityEditor;

namespace Match3.Scripts.Core
{
    [CreateAssetMenu(fileName = "LevelsDataBase", menuName = "Match3/Levels/Create LevelsDB")]
    public class LevelsDB : ScriptableObject
    {
        [SerializeField]    public List<LevelData> levels;

        public int GetLastLevelNumber()
        {
            return  levels.Count;
        }

        public LevelData GetLevel(int _levelNumber)
        {
            return  levels[_levelNumber-1].Clone();
        }
#if UNITY_EDITOR
        [ContextMenu(itemName:"Create New Level")]
        public void AddLevel()
        {
            var newLevel = new LevelData();
            newLevel.Height = 9;
            newLevel.Width = 9;
            newLevel.LevelNumber = GetLastLevelNumber()+1;
            newLevel.LevelName = $"Level {newLevel.LevelNumber}";
            newLevel.LimitType = LimitType.Moves;
            newLevel.LimitCount = 30;

            //Field
            newLevel.field = new Field();
            int size = newLevel.Width*newLevel.Height;
            newLevel.field.cells = new Field.Cell[size];
            newLevel.field.chips = new Field.Chip[size];
    
            for (int i = 0; i < newLevel.field.cells.Length; i++)
            {
                newLevel.field.cells[i] = new Field.Cell(Cells.CellType.EmptyBlock, Cells.BlockerType.None, Vector2.down, false);
                newLevel.field.chips[i] = new Field.Chip(Chips.ChipType.None, Chips.ChipBonusType.None);
            }

            //StartPoints
            for (int j = 0; j < newLevel.Width; j++)
            {
                int index = newLevel.Height * 0 + j;
                newLevel.field.cells[index].IsEnterPoint = true;
            }

            levels.Add(newLevel);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void EditLevel(int index, LevelData levelData)
        {
            var modifiedLevel = new LevelData();
            modifiedLevel.Height = levelData.Height;
            modifiedLevel.Width = levelData.Width;
            modifiedLevel.LevelNumber = levelData.LevelNumber;
            modifiedLevel.LevelName = levelData.LevelName;
            modifiedLevel.LimitType = levelData.LimitType;
            modifiedLevel.LimitCount = levelData.LimitCount;

            modifiedLevel.field = new Field();
            modifiedLevel.field.cells = new Field.Cell[levelData.field.cells.Length];
            modifiedLevel.field.chips = new Field.Chip[levelData.field.cells.Length];

            for (int i = 0; i < modifiedLevel.field.cells.Length; i++)
            {
                modifiedLevel.field.cells[i] = new Field.Cell(levelData.field.cells[i].cellType, levelData.field.cells[i].blockerType, levelData.field.cells[i].direction, levelData.field.cells[i].IsEnterPoint);
                modifiedLevel.field.chips[i] = new Field.Chip(levelData.field.chips[i].chipType, levelData.field.chips[i].chipBonusType);
            }

            levels[index-1] = modifiedLevel;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void RemoveLevel(int index)
        {
            levels.RemoveAt(index-1);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
