using System;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using Match3.Scripts.Core;
using Match3.Scripts.Save;
using Match3.Scripts.Levels;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Match3.Scripts.Editor
{
    [InitializeOnLoad]  public class LevelMakerEditor : EditorWindow
    {
        #region Graphics
            private ChipGraphics chipGraphics;
            private CellGraphics cellGraphics;
            private ChipForEditor[] chipsForEditor;
            private CellForEditor[] cellsForEditor;
            private ChipBlockSelected chipBlockSelected;
            private CellBlockSelected cellBlockSelected;
            private Vector2 cellDirectionSelected;
            private int selectDirectionType;
        #endregion
        private string[] sectionTitles = { Sections.Cells.ToString(), Sections.Chips.ToString(), Sections.Directions.ToString() }; 
        private Texture[] arrows = new Texture[4];
        private Texture[] arrows_enter = new Texture[4];   
        private Texture[] rotate_arrows = new Texture[2];
        private static LevelMakerEditor Instance;
        private LevelsDB levelsDB;
        private LevelData levelData;
        private int currentLevel;
        private int currentSectionNumber;
        private Sections currentSection;
        private bool levelDataChanged = false;
        private Vector2 scrollViewVector;

        
        [MenuItem("Match 3/Level editor")]
        public static void Init()
        {
            Instance = (LevelMakerEditor)GetWindow(typeof(LevelMakerEditor),false, "Level editor");
            Instance.Show();
            
        }

        public bool isFocused = false;

        private void OnFocus() 
        {
            if (isFocused)
                return;
            cellsForEditor = new CellForEditor[5];   
            chipsForEditor = new ChipForEditor[5];

            cellBlockSelected = new CellBlockSelected(CellType.None, BlockerType.None);
            chipBlockSelected = new ChipBlockSelected(ChipType.None, ChipBonusType.None);
            cellDirectionSelected = Vector2.down;
            selectDirectionType = 1;

            chipGraphics = (ChipGraphics)AssetDatabase.LoadAssetAtPath<ChipGraphics>("Assets/Match3/Resources/ChipGraphics.asset");
            cellGraphics = (CellGraphics)AssetDatabase.LoadAssetAtPath<CellGraphics>("Assets/Match3/Resources/CellGraphics.asset");
            levelsDB = (LevelsDB)AssetDatabase.LoadAssetAtPath<LevelsDB>("Assets/Match3/Resources/LevelsDataBase.asset");


            if (levelsDB == null)
                return;

            if (levelsDB.levels.Count == 0)
            {
                levelsDB.AddLevel();
                return;
            }

            currentLevel = 1;
            levelData = levelsDB.GetLevel(currentLevel);


            arrows[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Match3/Sprites/EditorSprites/arrow_blue_down.png",typeof(Texture));
            arrows[1] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Match3/Sprites/EditorSprites/arrow_blue_left.png",typeof(Texture));
            arrows[2] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Match3/Sprites/EditorSprites/arrow_blue_up.png",typeof(Texture));
            arrows[3] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Match3/Sprites/EditorSprites/arrow_blue_right.png",typeof(Texture));
            

            arrows_enter[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Match3/Sprites/EditorSprites/arrow_red_down.png",typeof(Texture));
            arrows_enter[1] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Match3/Sprites/EditorSprites/arrow_red_left.png",typeof(Texture));
            arrows_enter[2] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Match3/Sprites/EditorSprites/arrow_red_up.png",typeof(Texture));
            arrows_enter[3] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Match3/Sprites/EditorSprites/arrow_red_right.png",typeof(Texture));

            rotate_arrows[0] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Match3/Sprites/EditorSprites/circle arrow.png",typeof(Texture));
            rotate_arrows[1] = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Match3/Sprites/EditorSprites/circle arrows.png",typeof(Texture));


            if (chipsForEditor.Length != 0)
            {
                chipsForEditor = new ChipForEditor[chipGraphics.elements.Count];
                for (int i = 0; i < chipGraphics.elements.Count; i++)
                {
                    chipsForEditor[i] = new ChipForEditor(chipGraphics.elements[i]);
                }
            }
            if (cellsForEditor.Length != 0)
            {
                cellsForEditor = new CellForEditor[cellGraphics.elements.Count];
                for (int i = 0; i < cellGraphics.elements.Count; i++)
                {
                    cellsForEditor[i] = new CellForEditor(cellGraphics.elements[i]);
                }
            }
            isFocused = true;
        }

        private void OnGUI() 
        {
            if (levelDataChanged)
                SaveCurrentLevel();

            if (levelData == null)
            {
                OnFocus();
                return;
            }

            UnityEngine.GUI.changed = false;
            scrollViewVector = UnityEngine.GUI.BeginScrollView(new Rect(0, 0, position.width , position.height), scrollViewVector,new Rect(0, 0, 500, 1000));

            GUILayout.Space(10);    
            GUILayout.Label("\t\t\t\tEdit Level "+ currentLevel, EditorStyles.label, GUILayout.Width(400));

            GUILevelSelector();
            GUILayout.Space(10);

            GUILimit();
            GUILayout.Space(10);

            GUILevelSize();
            GUILayout.Space(10);

            GUISections();
            GUILayout.Space(10);

            GUITools();
            GUILayout.Space(10);

            GUIToolSelector();
            GUILayout.Space(10);

            GUIField();
            UnityEngine.GUI.EndScrollView();
        }

        #region Leveleditor
            private void GUILevelSelector()
            {
                GUILayout.BeginHorizontal();
                {

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Level", GUILayout.Width(150));
                    if (GUILayout.Button(new GUIContent("<<", "Перейти к предыдущему уровню"), GUILayout.Width(50)))
                    {
                        if (currentLevel > 1)
                            currentLevel--;
                        else
                            currentLevel = 1;
                        
                        levelData = levelsDB.GetLevel(currentLevel);
                    }
                    
                    string changeLvl = GUILayout.TextField(currentLevel.ToString(), GUILayout.Width(50));
                    try
                    {
                        if (int.Parse(changeLvl) != currentLevel)
                        {
                            currentLevel = int.Parse(changeLvl);
                            levelData = levelsDB.GetLevel(currentLevel);
                        }
                    }
                    catch (Exception) 
                    { 
                        DEBUGGER.DebugStr = $"Invalid value: Level Number";
                        DEBUGGER.Log(ColorType.Attention, DEBUGGER.DebugStr);
                        currentLevel = 1;
                        levelData = levelsDB.GetLevel(currentLevel);
                    }

                    
                    if (GUILayout.Button(new GUIContent(">>", "Перейти к следующему уровню"), GUILayout.Width(50)))
                    {
                        if (currentLevel < levelsDB.GetLastLevelNumber())
                            currentLevel++;
                        else
                            currentLevel = levelsDB.GetLastLevelNumber();
                        
                        levelData = levelsDB.GetLevel(currentLevel);
                    }

                    
                    if (GUILayout.Button(new GUIContent("+", "Добавить новый уровень"), GUILayout.Width(25)))
                    {
                        levelsDB.AddLevel();
                        currentLevel = levelsDB.GetLastLevelNumber();
                        levelData = levelsDB.GetLevel(currentLevel);
                        DEBUGGER.DebugStr = $"{levelData.LevelName} has been added";
                        DEBUGGER.Log(ColorType.Action, DEBUGGER.DebugStr);
                    }

                    if (GUILayout.Button(new GUIContent("-", "Удалить текущий уровень"), GUILayout.Width(25)))
                    {
                        DEBUGGER.DebugStr = $"{levelData.LevelName} has been removed";
                        DEBUGGER.Log(ColorType.Action, DEBUGGER.DebugStr);
                        levelsDB.RemoveLevel(currentLevel);
                        currentLevel--;
                        levelData = levelsDB.GetLevel(currentLevel);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            }


            private void GUILimit()
            {
                GUILayout.BeginHorizontal();
                {

                    GUILayout.Label("Limit", EditorStyles.label, GUILayout.Width(50));
                    GUILayout.Space(100);

                    LimitType limitTypeSave = levelData.LimitType;
                    int oldLimitCount = levelData.LimitCount;

                    levelData.LimitType = (LimitType)EditorGUILayout.EnumPopup(levelData.LimitType, GUILayout.Width(93));

                    if (levelData.LimitType == LimitType.Moves)
                        levelData.LimitCount = EditorGUILayout.IntField(levelData.LimitCount, GUILayout.Width(50));
                    else
                    {
                        GUILayout.BeginHorizontal();
                        int limitMin = EditorGUILayout.IntField(levelData.LimitCount / 60, GUILayout.Width(30));
                        GUILayout.Label(":", GUILayout.Width(10));
                        int limitSec = EditorGUILayout.IntField(levelData.LimitCount - (levelData.LimitCount / 60) * 60, GUILayout.Width(30));
                        levelData.LimitCount = limitMin * 60 + limitSec;
                        GUILayout.EndHorizontal();
                    }

                    if (levelData.LimitCount <= 0)
                        levelData.LimitCount = 1;

                    if (limitTypeSave != levelData.LimitType || oldLimitCount != levelData.LimitCount)
                        levelDataChanged = true;
                }

                GUILayout.EndHorizontal();
                GUILayout.Space(5);
            }

            private void GUILevelSize()
            {
                int oldWidth = levelData.Width;
                int oldHeight = levelData.Height;

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Width", GUILayout.Width(EditorGUIUtility.labelWidth));
                levelData.Width = EditorGUILayout.IntField(levelData.Width, GUILayout.Width(50));
                GUILayout.EndHorizontal();
                if (levelData.Width <= 0) levelData.Width = 1;
                if (levelData.Width >= 20) levelData.Width = 20;

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Height", GUILayout.Width(EditorGUIUtility.labelWidth));
                levelData.Height = EditorGUILayout.IntField(levelData.Height, GUILayout.Width(50));
                GUILayout.EndHorizontal();
                if (levelData.Height <= 0) levelData.Height = 1;
                if (levelData.Height >= 20) levelData.Height = 20;

                if (oldWidth != levelData.Width || oldHeight != levelData.Height)
                {
                    ClearField();
                }
            }

            private void GUIField()
            {
                GUILayout.Label("Field: ", EditorStyles.boldLabel);

                if (currentSection == Sections.Cells)
                {
                    for (int i = 0; i < levelData.Height; i++)
                    {
                        GUILayout.BeginHorizontal();
                        for (int j = 0; j < levelData.Width; j++)
                        {
                            var index = levelData.Width * i + j;
                            var element = cellGraphics.GetElement(levelData.field.cells[index].cellType, levelData.field.cells[index].blockerType);
                            Texture cellTexture = element.Sprite.texture;
                            if (GUILayout.Button(new GUIContent(cellTexture, element.Name), GUILayout.Width(50), GUILayout.Height(50)))
                            {
                                levelData.field.cells[index] = new Field.Cell(cellBlockSelected.cellType, cellBlockSelected.blockerType, levelData.field.cells[index].direction, levelData.field.cells[index].IsEnterPoint);
                                levelDataChanged = true;
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                if (currentSection == Sections.Chips)
                {
                    for (int i = 0; i < levelData.Height; i++)
                    {
                        GUILayout.BeginHorizontal();
                        for (int j = 0; j < levelData.Width; j++)
                        {
                            var index = levelData.Width * i + j;
                            if (levelData.field.chips[index].chipType == ChipType.None && levelData.field.chips[index].chipBonusType == ChipBonusType.None)
                            {
                                Texture chipTexture = cellGraphics.GetSprite(CellType.None, BlockerType.None).texture;
                                if (GUILayout.Button(new GUIContent(chipTexture, "Random Chip"), GUILayout.Width(50), GUILayout.Height(50)))
                                {
                                    levelData.field.chips[index] = new Field.Chip(chipBlockSelected.chipType, chipBlockSelected.chipBonusType);
                                    levelDataChanged = true;
                                }
                            }
                            else
                            {
                                var element = chipGraphics.GetElement(levelData.field.chips[index].chipType, levelData.field.chips[index].chipBonusType);
                                Texture chipTexture = element.Sprite.texture;
                                if (GUILayout.Button(new GUIContent(chipTexture, element.Name), GUILayout.Width(50), GUILayout.Height(50)))
                                {
                                    levelData.field.chips[index] = new Field.Chip(chipBlockSelected.chipType, chipBlockSelected.chipBonusType);
                                    levelDataChanged = true;
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                if (currentSection == Sections.Directions)
                {
                    for (int i = 0; i < levelData.Height; i++)
                    {
                        GUILayout.BeginHorizontal();
                        for (int j = 0; j < levelData.Width; j++)
                        {
                            var index = levelData.Width * i + j;
                            Texture directTexture = GetTexture(levelData.field.cells[index].direction, levelData.field.cells[index].IsEnterPoint);
                            if (GUILayout.Button(new GUIContent(directTexture, "Arrow"), GUILayout.Width(50), GUILayout.Height(50)))
                            {
                                if (selectDirectionType == 0)
                                {
                                    int rotate_index = GetIndexByDirection(levelData.field.cells[index].direction);
                                    rotate_index = (int)Mathf.Repeat(rotate_index + 1, 4);
                                    levelData.field.cells[index].direction = GetDirectionByIndex(rotate_index);
                                    levelDataChanged = true;
                                }
                                else if (selectDirectionType == 1)
                                {
                                    levelData.field.cells[index].IsEnterPoint = !levelData.field.cells[index].IsEnterPoint;
                                    levelDataChanged = true;
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
            }
        #endregion
        
        #region  Level Editor Sections
            private void GUISections()
            {
                GUILayout.BeginHorizontal();
                {
                    currentSectionNumber = GUILayout.Toolbar(currentSectionNumber, sectionTitles, GUILayout.Width(450));

                    if (currentSectionNumber == 0)
                        currentSection = Sections.Cells;
                    if (currentSectionNumber == 1)
                        currentSection = Sections.Chips;
                    if (currentSectionNumber == 2)
                        currentSection = Sections.Directions;
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
                GUILayout.Label("Tip: Click to change level element", EditorStyles.boldLabel);
            }

            private void GUITools()
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.Label("Tools:", EditorStyles.boldLabel);
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button(new GUIContent("Clear", "Очистить уровень"), GUILayout.Width(50), GUILayout.Height(50)))
                        {
                            ClearField();
                            DEBUGGER.DebugStr = $"{levelData.LevelName} has been cleaned";
                            DEBUGGER.Log(ColorType.Action, DEBUGGER.DebugStr);
                        }
                        GUILayout.BeginVertical();
                        {
                            if (GUILayout.Button(new GUIContent("X", "Очистить блок"), GUILayout.Width(50), GUILayout.Height(50)))
                            {
                                if (currentSection == Sections.Cells)
                                    cellBlockSelected = new CellBlockSelected(CellType.EmptyBlock , BlockerType.None);
                                if (currentSection == Sections.Chips)
                                    chipBlockSelected = new ChipBlockSelected(ChipType.None, ChipBonusType.None);
                                if (currentSection == Sections.Directions)
                                {
                                    selectDirectionType = 1;
                                    cellDirectionSelected = Vector2.down;
                                }
                                DEBUGGER.DebugStr = $"Selected Tool: Clear One";
                                DEBUGGER.Log(ColorType.Other, DEBUGGER.DebugStr);
                            }
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }


            private void GUIToolSelector()
            {
                GUILayout.BeginVertical();
                {
                    if (currentSection == Sections.Cells)
                    {
                        GUILayout.Label("Common Cells:", EditorStyles.boldLabel);
                        GUILayout.BeginHorizontal();
                        {
                            foreach (var editorElement in cellsForEditor)
                            {
                                if (editorElement.cellElement.BlockerType == BlockerType.None)
                                {
                                    Texture cellTexture = editorElement.cellElement.Sprite.texture;
                                    if (GUILayout.Button(new GUIContent(cellTexture, editorElement.cellElement.Name), GUILayout.Width(50), GUILayout.Height(50)))
                                    {
                                        cellBlockSelected = new CellBlockSelected(editorElement.cellElement.CellType, editorElement.cellElement.BlockerType);
                                        DEBUGGER.DebugStr = $"Selected Cell: {editorElement.cellElement.Name}";
                                        DEBUGGER.Log(ColorType.Other, DEBUGGER.DebugStr);
                                    }
                                }
                            }
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.Label("Blocker Cells:", EditorStyles.boldLabel);
                        GUILayout.BeginHorizontal();
                        {
                            foreach (var editorElement in cellsForEditor)
                            {
                                if (editorElement.cellElement.BlockerType != BlockerType.None)
                                {
                                    Texture cellTexture = editorElement.cellElement.Sprite.texture;
                                    if (GUILayout.Button(new GUIContent(cellTexture, editorElement.cellElement.Name), GUILayout.Width(50), GUILayout.Height(50)))
                                    {
                                        cellBlockSelected = new CellBlockSelected(editorElement.cellElement.CellType, editorElement.cellElement.BlockerType);
                                        DEBUGGER.DebugStr = $"Selected Blocker: {editorElement.cellElement.Name}";
                                        DEBUGGER.Log(ColorType.Other, DEBUGGER.DebugStr);
                                    }
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                    }

                    if (currentSection == Sections.Chips)
                    {
                        GUILayout.Label("Common Chips:", EditorStyles.boldLabel);
                        GUILayout.BeginHorizontal();
                        {
                            foreach (var editorElement in chipsForEditor)
                            {
                                if (editorElement.chipElement.ChipType != ChipType.None && editorElement.chipElement.ChipBonusType == ChipBonusType.None)
                                {
                                    Texture chipTexture = editorElement.chipElement.Sprite.texture;
                                    if (GUILayout.Button(new GUIContent(chipTexture, editorElement.chipElement.Name), GUILayout.Width(50), GUILayout.Height(50)))
                                    {
                                        chipBlockSelected = new ChipBlockSelected(editorElement.chipElement.ChipType, editorElement.chipElement.ChipBonusType);
                                        DEBUGGER.DebugStr = $"Selected Chip: {editorElement.chipElement.Name}";
                                        DEBUGGER.Log(ColorType.Other, DEBUGGER.DebugStr);
                                    }
                                }
                            }
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.Label("Bonus Chips:", EditorStyles.boldLabel);
                        GUILayout.BeginHorizontal();
                        {
                            foreach (var editorElement in chipsForEditor)
                            {
                                if (editorElement.chipElement.ChipType == ChipType.None && editorElement.chipElement.ChipBonusType != ChipBonusType.None)
                                {
                                    Texture chipTexture = editorElement.chipElement.Sprite.texture;
                                    if (GUILayout.Button(new GUIContent(chipTexture, editorElement.chipElement.Name), GUILayout.Width(50), GUILayout.Height(50)))
                                    {
                                        chipBlockSelected = new ChipBlockSelected(editorElement.chipElement.ChipType, editorElement.chipElement.ChipBonusType);
                                        DEBUGGER.DebugStr = $"Selected Chip: {editorElement.chipElement.Name}";
                                        DEBUGGER.Log(ColorType.Other, DEBUGGER.DebugStr);
                                    }
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                if (currentSection == Sections.Directions)
                {
                    GUILayout.Label("Directions:", EditorStyles.boldLabel);
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button(new GUIContent(arrows_enter[0], "Стартовая позиция ON/OFF"), GUILayout.Width(50), GUILayout.Height(50)))
                        {
                            selectDirectionType = 1;
                            DEBUGGER.DebugStr = $"Selected Tool: Start Pos";
                            DEBUGGER.Log(ColorType.Other, DEBUGGER.DebugStr);
                        }
                    
                        if (GUILayout.Button(new GUIContent(rotate_arrows[0], "Повернуть одну стрелку"), GUILayout.Width(50), GUILayout.Height(50)))
                        {
                            selectDirectionType = 0;
                            DEBUGGER.DebugStr = $"Selected Tool: Rotate One";
                            DEBUGGER.Log(ColorType.Other, DEBUGGER.DebugStr);
                        }

                        if (GUILayout.Button(new GUIContent(rotate_arrows[1], "Повернуть все стрелки"), GUILayout.Width(50), GUILayout.Height(50)))
                        {
                            RotateAllDirections();
                            DEBUGGER.DebugStr = $"All directions has been rotated";
                            DEBUGGER.Log(ColorType.Action, DEBUGGER.DebugStr);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }

            private Texture GetTexture(Vector2 direction, bool IsEnterPoint)
            {
                if (!IsEnterPoint)
                    return arrows[GetIndexByDirection(direction)];
                else
                    return arrows_enter[GetIndexByDirection(direction)];
            }

            private int GetIndexByDirection(Vector2 direction)
            {
                if (direction == Vector2.right)
                    return 3;
                if (direction == Vector2.up)
                    return 2;
                if (direction == Vector2.left)
                    return 1;
                return 0;
            }
            private Vector2 GetDirectionByIndex(int index)
            {
                if (index == 0) return Vector2.down;
                if (index == 1) return Vector2.left;
                if (index == 2) return Vector2.up;
                if (index == 3) return Vector2.right;
                return Vector2.down;
            }

            public void RotateAllDirections()
            {
                int size = levelData.Width * levelData.Height;

                
                for (int i = 0; i < levelData.field.cells.Length; i++)
                {
                    int cur_arrow_index = GetIndexByDirection(levelData.field.cells[i].direction);
                    cur_arrow_index = (int)Mathf.Repeat(cur_arrow_index + 1, 4);

                    levelData.field.cells[i] = new Field.Cell(levelData.field.cells[i].cellType, levelData.field.cells[i].blockerType, GetDirectionByIndex(cur_arrow_index), levelData.field.cells[i].IsEnterPoint);
                }

                levelDataChanged = true;
            }




            private void ClearField()
            {
                levelData.field = new Field();
                levelData.field.cells = new Field.Cell[levelData.Height * levelData.Width];
                levelData.field.chips = new Field.Chip[levelData.Height * levelData.Width];

                for (int i = 0; i < levelData.field.cells.Length; i++)
                {
                    levelData.field.cells[i] = new Field.Cell(Cells.CellType.EmptyBlock, Cells.BlockerType.None,  Vector2.down,  false);
                    levelData.field.chips[i] = new Field.Chip(Chips.ChipType.None, Chips.ChipBonusType.None);
                }

                for (int j = 0; j < levelData.Width; j++)
                {
                    int index = levelData.Height * 0 + j;
                    levelData.field.cells[index].IsEnterPoint = true;
                }
                levelDataChanged = true;
                SaveCurrentLevel();
            }
        #endregion
        
        #region Level Functions
            private void SaveCurrentLevel()
            {
                if (levelDataChanged)
                {
                    levelsDB.EditLevel(currentLevel, levelData);
                    levelDataChanged = false;
                }
            }
        #endregion
    }

    public enum Sections
    {
        Cells = 0,
        Chips,
        Directions
    }
}
