using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using Match3.Scripts.Combination.MultipleBonus;
using Match3.Scripts.Combination.Matches;
using Match3.Scripts.Combination.Bonus;
using Match3.Scripts.Levels;
using Match3.Scripts.UI;
using UnityEngine;
using Random = System.Random;


namespace Match3.Scripts.Core
{
    public class GameField : MonoBehaviour
    {
        #region  level variables
        private Random random = new Random();
        private static GameField _instance;
        [SerializeField] private GameScene gameScene;
        [SerializeField] private GameUi gameUi;
        [SerializeField] private Transform Fields;
        [SerializeField] private Transform PooledObjects;
        [SerializeField] public Transform Effects;
        [SerializeField] private Transform boardCenter;
        private Vector2 firstCellPosition = new Vector2(-2.4f, 3.3f);
        private float cellWidth = 2f;
        private float cellHeight = 2f;
        public FieldHandler fieldHandler;
        public Cell[] cells = new Cell[5];
        public LevelData levelData;
        public int currentLimit;
        #endregion
        public List<Cell> spawnCells;
        private Coroutine countdownCoroutine;
        
        public static GameField Instance
        {
            get
            {
                return _instance;
            }
        }

        private void Awake()
        {
            Assert.IsNotNull(gameScene);
            Assert.IsNotNull(gameUi);
            if(_instance == null)
            {
                _instance = this;
            }
        }

        public void LoadLevel()
        {
            Debug.Log(LinkKeeper.currentLevel);
            levelData = AssetLoader.Instance.levelsDB.GetLevel(LinkKeeper.currentLevel);
            ReloadLevelData();
            while (GetMatches().Count > 0)
            {
                ReloadLevelData();
            }
            
            
            spawnCells = DetectSpawnCells();
            //DEBUGGER.ClearConsole();
            FixCamera();
            
            
            DEBUGGER.DebugStr = $"Load {levelData.LevelName}. Size:[{levelData.Width}][{levelData.Height}]. Limit: {levelData.LimitType.ToString()}[{levelData.LimitCount}].";
            DEBUGGER.Log(ColorType.System, DEBUGGER.DebugStr);
            fieldHandler.IntitalizeField();
            gameScene.StartGame();
        }

        private void FixCamera()
        {
            var totalWidth = (levelData.Width - 1) * cellWidth;
            var totalHeight = (levelData.Height - 1) * cellHeight;
            Camera.main.transform.position = GetCenter();
            Camera.main.orthographicSize = (totalWidth * 1.5f) * (Screen.height / (float)Screen.width) * 1f;//8
            // DEBUGGER.Log($"Camera: "+ Camera.main.orthographicSize);
            // if (levelData.Width > levelData.Height)
            //     Camera.main.orthographicSize = (totalWidth * 1.5f) * (Screen.height / (float)Screen.width) * 0.5f;//16
            // else if (levelData.Width < levelData.Height)
            //     Camera.main.orthographicSize = (totalWidth * 1.5f) * (Screen.height / (float)Screen.width) * 0.5f;//12
            // else
            //     Camera.main.orthographicSize = (totalWidth * 1.5f) * (Screen.height / (float)Screen.width) * 0.5f;//8
            // DEBUGGER.Log($"Camera: "+ Camera.main.orthographicSize);
        }

        private Vector3 GetCenter()
        {
            Vector3 sum = Vector3.zero;
            foreach (var cell in cells)
            {
                sum += cell.transform.position;
            }
            sum = sum/(levelData.Height * levelData.Width);
            return sum;
        }


        public void ReloadLevelData()
        {
            ClearGameField();

            gameUi.SetLimit(levelData.LimitCount);
            gameUi.SetLimitType(levelData.LimitType);

            //fieldHandler.ResetSwap();
            currentLimit = levelData.LimitCount;
            
            
            cells = new Cell[levelData.Height * levelData.Width];
            
            for (int row = 0; row < levelData.Height; row++)
            {
                for (int column = 0; column < levelData.Width; column++)
                {
                    var cellData = levelData.field.cells[row * levelData.Width + column];
                    var chipData = levelData.field.chips[row * levelData.Width + column];
                    

                    CellElement element = AssetLoader.Instance.cellGraphics.GetElement(cellData.cellType, cellData.blockerType);
                    GameObject obj = Instantiate(AssetLoader.Instance.CellPrefab, Fields);
                    Cell cell = obj.GetComponent<Cell>();
                    cell.Position = new Vector2Int(row, column);
                    cell.CellType = element.CellType;
                    cell.CellBlockerType = element.BlockerType;
                    cell.IsEnterPoint = cellData.IsEnterPoint;
                    cell.Direction = cellData.direction;
                    cell.transform.localPosition = firstCellPosition + new Vector2(column * cellWidth, -row * cellHeight);
                    if (cell.CanChipSet)
                    {
                        if (chipData.chipType == ChipType.None && chipData.chipBonusType == ChipBonusType.None)
                            cell.SetChip(CreateRandomChip());
                        else
                            cell.SetChip(CreateChip(chipData.chipType, chipData.chipBonusType));
                        cell.chip.transform.position = cell.transform.position;
                    }
                    cell.Initialize();
                    cells[row * levelData.Width + column] = cell;
                }
            }
            
        }

        private void ClearGameField()
        {
            if (cells.Length != 0)
            {
                foreach (var cell in cells)
                {
                    cell.DestroyChip();
                    cell.DestroyCell();
                }
                cells = new Cell[5];
            }
        }

        #region  Game Control
        public void StartGame()
        {
            if (levelData.LimitType == LimitType.Time)
            {
                countdownCoroutine = StartCoroutine(StartCountdown());
            }
        }

        public void EndGame()
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
            }
        }

        private IEnumerator StartCountdown()
        {
            while (currentLimit > 0)
            {
                --currentLimit;
                UpdateLimitText();
                yield return new WaitForSeconds(1.0f);
            }

            gameScene.CheckEndGame();
        }

        private void UpdateLimitText()
        {
            if (levelData.LimitType == LimitType.Moves)
            {
                gameUi.SetLimit(currentLimit);
            }
            else if (levelData.LimitType == LimitType.Time)
            {
                var timeSpan = TimeSpan.FromSeconds(currentLimit);
                gameUi.SetLimit(string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds));
            }
        }

        public void PerformMove()
        {
            if (levelData.LimitType == LimitType.Moves)
            {
                currentLimit -= 1;
                if (currentLimit < 0)
                {
                    currentLimit = 0;
                }

                gameUi.SetLimit(currentLimit);
                gameScene.CheckEndGame();
            }
        }
        #endregion

        #region Creators
        public Chip CreateChip(ChipType type, ChipBonusType bonusType)
        {
            ChipElement element = AssetLoader.Instance.chipGraphics.GetElement(type, bonusType);
            GameObject obj = Instantiate(AssetLoader.Instance.ChipPrefab, PooledObjects.transform);
            Chip chip = obj.GetComponent<Chip>();

            chip.ChipType = element.ChipType;
            chip.ChipBonusType = element.ChipBonusType;

            return chip;
        }


        public Chip CreateRandomChip()
        {
            ChipElement element = AssetLoader.Instance.chipGraphics.GetRandomElement();
            GameObject obj = Instantiate(AssetLoader.Instance.ChipPrefab, PooledObjects.transform);
            Chip chip = obj.GetComponent<Chip>();

            chip.ChipType = element.ChipType;
            chip.ChipBonusType = element.ChipBonusType;

            return chip;
        }

        public void CreateBonusChip(int row, int column, ChipBonusType bonusType)
        {
            var chip = GetChip(row, column);

            if (bonusType != ChipBonusType.None)
            {
                chip.nextType = ChipType.None;
                chip.nextBonusType = bonusType;
                chip.State = ChipState.BonusAppear;
            }
            else
            {
                chip.State = ChipState.Destroy;
            }
        }

        public void RespawnChips()
        {
            foreach (var cell in spawnCells)
            {
                if (cell.chip != null)
                    continue;
                cell.SetChip(CreateRandomChip());
                cell.chip.transform.position = cell.transform.position;
            }
        }

        private void Shuffle<T>(T[] arr) // перемешивание элементов массива
        {
            System.Random rand = new System.Random();

            for (int i = arr.Length - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                T tmp = arr[j];
                arr[j] = arr[i];
                arr[i] = tmp;
            }
        }
        #endregion

        #region GetCells
        public Cell GetCell(int row, int column, bool safe = false)
        {
            if (!safe)
            {
                if (row >= levelData.Height || column >= levelData.Width || row < 0 || column < 0)
                    return null;
                return cells[row * levelData.Width + column];
            }

            return cells[row * levelData.Width + column];
        }

        public Cell GetNeighborLeft(int row, int column)
        {
            if (column == 0)
                return null;

            var cell = GetCell(row, column - 1);
            return cell;
        }

        public Cell GetNeighborRight(int row, int column)
        {
            if (column >= levelData.Width-1)
                return null;

            var cell = GetCell(row, column + 1);
            return cell;
        }

        public Cell GetNeighborTop(int row, int column)
        {
            if (row == 0)
                return null;

            var cell = GetCell(row - 1, column);
            return cell;
        }

        public Cell GetNeighborBottom(int row, int column)
        {
            if (row >= levelData.Height-1)
                return null;

            var cell = GetCell(row + 1, column);
            return cell;
        }
        public Cell GetNeighborCell(int row, int column, Vector2 direction)
        {
            if (direction == Vector2.down)
                return GetNeighborBottom(row, column);
            if (direction == Vector2.up)
                return GetNeighborTop(row, column);
            if (direction == Vector2.left)
                return GetNeighborLeft(row, column);
            if (direction == Vector2.right)
                return GetNeighborRight(row, column);
            else return null;
        }

        public List<Cell> DetectSpawnCells()
        {
            var cellsList = new List<Cell>();
            foreach (var cell in cells)
            {
                if (cell.IsEnterPoint)
                {
                    cellsList.Add(cell);
                }
            }
            return cellsList;
        }
        #endregion

        #region GetChips
        public Chip GetChip(int col, int row, bool safe = false)
        {
            var cell = GetCell(col, row, safe);
            if (cell != null)
            {
                if (cell.chip != null)
                    return cell.chip;
            }

            return null;
        }

        public Chip GetLeftChip(int row, int column)
        {
            var cell = GetNeighborLeft(row, column);
            if (cell != null)
                return GetChip(cell.Position.x, cell.Position.y);
            else
                return null;
        }

        public Chip GetRightChip(int row, int column)
        {
            var cell = GetNeighborRight(row, column);
            if (cell != null)
                return GetChip(cell.Position.x, cell.Position.y);
            else
                return null;
        }

        public Chip GetTopChip(int row, int column)
        {
            var cell = GetNeighborTop(row, column);
            if (cell != null)
                return GetChip(cell.Position.x, cell.Position.y);
            else
                return null;
        }

        public Chip GetBottomChip(int row, int column)
        {
            var cell = GetNeighborBottom(row, column);
            if (cell != null)
                return GetChip(cell.Position.x, cell.Position.y);
            else
                return null;
        }

        public Chip GetNeighborChip(int row, int column, Vector2 direction)
        {
            if (direction == Vector2.down)
                return GetBottomChip(row, column);
            if (direction == Vector2.up)
                return GetTopChip(row, column);
            if (direction == Vector2.left)
                return GetLeftChip(row, column);
            if (direction == Vector2.right)
                return GetRightChip(row, column);
            else return null;
        }
        #endregion

        #region GetCombo
        public void ActivateBonus(Chip bonusChip)
        {
            BonusCombiner bonusCombiner = new BonusCombiner();
            bonusCombiner.Check(bonusChip, CreateRandomChip());
        }
        public Chip GetRandomChip()
        {
            
            var cell = GetCell(random.Next(0, levelData.Height), random.Next(0, levelData.Width));
            if (cell != null)
            {
                while (cell == null)
                {
                    cell = GetCell(random.Next(0, levelData.Height), random.Next(0, levelData.Width));
                    if (cell.chip == null)
                    {
                        cell = GetCell(random.Next(0, levelData.Height), random.Next(0, levelData.Width));
                    }
                }
            }

            return cell.chip;
        }   
        public List<Chip> GetRow(int row)
        {
            List <Chip> chips = new List<Chip>();
            for (int rowElement = 0; rowElement < levelData.Width; rowElement++)
            {
                var chip = GetChip(row, rowElement);
                if (chip != null)
                    chips.Add(chip);
            }

            return chips;
        }

        public List<Chip> GetColumn(int column)
        {
            List <Chip> chips = new List<Chip>();
            for (int columnElement = 0; columnElement < levelData.Height; columnElement++)
            {
                var chip = GetChip(columnElement, column);
                if (chip != null)
                    chips.Add(chip);
            }

            return chips;
        }

        public List<Chip> GetNeighbors(int row, int column)
        {
            var chipsList = new List<Chip>();

            if (GetTopChip(row, column) != null) 
                chipsList.Add(GetTopChip(row, column));

            if (GetBottomChip(row, column) != null) 
                chipsList.Add(GetBottomChip(row, column));

            if (GetLeftChip(row, column) != null) 
                chipsList.Add(GetLeftChip(row, column));

            if (GetRightChip(row, column) != null) 
                chipsList.Add(GetRightChip(row, column));

            var randomCell = GetCell(UnityEngine.Random.Range(0, levelData.Height-1), UnityEngine.Random.Range(0, levelData.Width-1));
            if (randomCell != null && randomCell.chip != null) 
                chipsList.Add(randomCell.chip);
            
            return chipsList;
        }

        public List<Chip> GetByType(ChipType type)
        {
            var chipsByType = new List<Chip>();

            foreach (var cell in cells)
            {
                if (cell == null || cell.chip == null)
                    continue;
                if (cell.chip.ChipType == type)
                {
                    chipsByType.Add(cell.chip);
                }
            }
            return chipsByType;
        }

        public List<Chip> GetChipsAroundFirst(int row, int column)
        {
            var chipsList = new List<Chip>();
            if (GetChip(row + 0, column - 1) != null) 
            chipsList.Add(GetCell(row + 0, column - 1)?.chip);
            
            if (GetChip(row + 1, column - 1) != null) 
            chipsList.Add(GetCell(row + 1, column - 1)?.chip);

            if (GetChip(row + 1, column + 0) != null) 
            chipsList.Add(GetCell(row + 1, column + 0)?.chip);

            if (GetChip(row + 1, column + 1) != null) 
            chipsList.Add(GetCell(row + 1, column + 1)?.chip);

            if (GetChip(row + 0, column + 1) != null) 
            chipsList.Add(GetCell(row + 0, column + 1)?.chip);//

            if (GetChip(row - 1, column + 1) != null) 
            chipsList.Add(GetCell(row - 1, column + 1)?.chip);

            if (GetChip(row - 1, column + 0) != null) 
            chipsList.Add(GetCell(row - 1, column + 0)?.chip);

            if (GetChip(row - 1, column - 1) != null) 
            chipsList.Add(GetCell(row - 1, column - 1)?.chip);

            return chipsList;
        }


        public List<Chip> GetChipsAroundSecond(int row, int column)
        {        
            var chipsList = new List<Chip>();

            var r = row - 2;
            var c = column - 2;

            for (c = column - 2; c <= column + 2; c++)
            {
                if (GetCell(c, r)?.chip != null) 
                chipsList.Add(GetCell(c, r)?.chip);
            }

            c = column + 2;
            for (r = row - 1; r <= row + 2; r++)
            {
                if (GetCell(c, r)?.chip != null) 
                chipsList.Add(GetCell(c, r)?.chip);
            }

            r = row + 2;
            for (c = column + 1; c >= column - 2; c--)
            {
                if (GetCell(c, r)?.chip != null) 
                chipsList.Add(GetCell(c, r)?.chip);
            }

            c = column - 2;
            for (r = row + 1; r >= row - 1; r--)
            {
                if (GetCell(c, r)?.chip != null) 
                chipsList.Add(GetCell(c, r)?.chip);
            }

            return chipsList;
        }

        public List<Chip> GetChipsAroundThird(int row, int column)
        {
            var chipsList = new List<Chip>();

            var r = row - 3;
            var c = column - 3;

            for (c = column - 3; c <= column + 3; c++)
            {
                if (GetCell(c, r)?.chip != null) 
                chipsList.Add(GetCell(c, r)?.chip);
            }

            c = column + 3;
            for (r = row - 2; r <= row + 3; r++)
            {
                if (GetCell(c, r)?.chip != null) 
                chipsList.Add(GetCell(c, r)?.chip);
            }

            r = row + 3;
            for (c = column + 2; c >= column - 3; c--)
            {
                if (GetCell(c, r)?.chip != null) 
                chipsList.Add(GetCell(c, r)?.chip);
            }

            c = column - 3;
            for (r = row + 2; r >= row - 2; r--)
            {
                if (GetCell(c, r)?.chip != null) 
                chipsList.Add(GetCell(c, r)?.chip);
            }

            return chipsList;
        }
        #endregion

        public List<Match> GetMatches()
        {
            MatchDetector matchDetector = new MatchDetector();
            return matchDetector.GetMatches();
        }
    }
}
