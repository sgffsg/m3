using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using Match3.Scripts.Combination.Matches;
using Match3.Scripts.Combination.MultipleBonus;
using Match3.Scripts.Combination.Bonus;
using UnityEngine.Assertions;
using UnityEngine;
using System;

namespace Match3.Scripts.Core
{
    public class FieldHandler : MonoBehaviour
    {
        private static FieldHandler _instance;
        [SerializeField] private GameField gameField;
        private void Awake() 
        {
            Assert.IsNotNull(gameField);
            if(_instance == null)
            {
                _instance = this;
            }
        }

        #region  Input variables
        public Chip selectedChip;
        public Chip lastselectedChip;
        public MatchDetector matchDetector = new MatchDetector();
        public BonusCombiner bonusCombiner = new BonusCombiner();
        public MultipleBonusCombiner multipleBonusCombiner = new MultipleBonusCombiner();
        private Vector3 mousePos = Vector3.zero;
        private Vector3 deltaPos = Vector3.zero;
        private Vector3 switchDirection = Vector3.zero;
        public bool currentlySwapping;
        public bool inputLocked;
        #endregion

        #region Input Handle
        public void HandleInput()
        {
            if (currentlySwapping)
            {
                return;
            }

            if (inputLocked)
            {
                return;
            }


            if (Input.GetMouseButtonDown(0))
            {
                if (selectedChip == null)
                {
                    ResetSwap();
                    var hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1 << LayerMask.NameToLayer("Chip"));
                    if (hit != null)
                    {
                        if (hit.gameObject.GetComponent<Chip>().State != ChipState.Idle)
                        {
                            return;
                        }

                        mousePos = GetMousePosition();
                        selectedChip = hit.gameObject.GetComponent<Chip>();
                        hit.gameObject.GetComponent<Animator>().SetTrigger("Pressed");
                    }
                }
                else
                {
                    var hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1 << LayerMask.NameToLayer("Chip"));
                    if (hit != null)
                    {
                        if (hit.gameObject.GetComponent<Chip>().State != ChipState.Idle)
                        {
                            return;
                        }

                        lastselectedChip = hit.gameObject.GetComponent<Chip>();
                        if (selectedChip == lastselectedChip)
                        {
                            Chip testChip = new Chip();
                            testChip.ChipBonusType = ChipBonusType.None;
                            testChip.ChipType = (ChipType)UnityEngine.Random.Range(0, (int)ChipType.Chip5);
                            if (bonusCombiner.Check(selectedChip, testChip))
                            {
                                HandleMatches(true);
                                gameField.PerformMove();
                                ResetSwap();
                            }
                            else ResetSwap();
                        }
                        else
                        {
                            if (!CheckNeighbor())
                            {
                                ResetSwap();
                                selectedChip = hit.gameObject.GetComponent<Chip>();
                            }
                            else ExecuteMatch();
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (selectedChip != null && lastselectedChip == null)
                {
                    selectedChip.gameObject.GetComponent<Animator>().SetTrigger("UnPressed");
                    deltaPos = mousePos - GetMousePosition();
                    if (GetNeighbor() != null)
                    {
                        lastselectedChip = GetNeighbor();
                        if (!CheckNeighbor())
                        {
                            ResetSwap();
                        }
                        else ExecuteMatch();
                    }
                }
            }
        }

        public void ExecuteMatch()
        {
            if (selectedChip != null && lastselectedChip != null)
            {
                Debug.Log("Check Swap");
                var startingCell = GetCell(selectedChip.Position.x, selectedChip.Position.y);
                var destinationCell = GetCell(lastselectedChip.Position.x, lastselectedChip.Position.y);
                var firstChip = GetChip(startingCell.Position.x, startingCell.Position.y);
                var secondChip = GetChip(destinationCell.Position.x, destinationCell.Position.y);

                startingCell.SetChip(secondChip);
                destinationCell.SetChip(firstChip);
                SwapPositions(startingCell.chip.transform, destinationCell.chip.transform, 0.25f, () =>
                {
                    if (bonusCombiner.Check(startingCell.chip, destinationCell.chip))
                    {
                        HandleMatches(true);
                        gameField.PerformMove();
                        ResetSwap();
                    }
                    else if (multipleBonusCombiner.Check(startingCell.chip, destinationCell.chip))
                    {
                        HandleMatches(true);
                        gameField.PerformMove();
                        ResetSwap();
                    }
                    else if (GetMatches().Count > 0)
                    {
                        HandleMatches(true);
                        gameField.PerformMove();
                        ResetSwap();
                        return;
                    }
                    else
                    {
                        SwapPositions(startingCell.chip.transform, destinationCell.chip.transform, 0.25f, () =>
                        {
                            startingCell.SetChip(firstChip);
                            destinationCell.SetChip(secondChip);
                            ResetSwap();
                        });
                    }
                });
            }
        }

        private Chip GetNeighbor()
        {
            if (Vector3.Magnitude(deltaPos) > 0.1f)
            {
                if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y) && deltaPos.x > 0)
                    switchDirection.x = 1;
                else if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y) && deltaPos.x < 0)
                    switchDirection.x = -1;
                else if (Mathf.Abs(deltaPos.x) < Mathf.Abs(deltaPos.y) && deltaPos.y > 0)
                    switchDirection.y = 1;
                else if (Mathf.Abs(deltaPos.x) < Mathf.Abs(deltaPos.y) && deltaPos.y < 0)
                    switchDirection.y = -1;
                
                Chip neighborChip = null;
                if (switchDirection.x > 0)
                    neighborChip = gameField.GetLeftChip(selectedChip.Position.x, selectedChip.Position.y);
                else if (switchDirection.x < 0)
                    neighborChip = gameField.GetRightChip(selectedChip.Position.x, selectedChip.Position.y);
                else if (switchDirection.y > 0)
                    neighborChip = gameField.GetBottomChip(selectedChip.Position.x, selectedChip.Position.y);
                else if (switchDirection.y < 0)
                    neighborChip = gameField.GetTopChip(selectedChip.Position.x, selectedChip.Position.y);
                return neighborChip;
            }
            return null;
        }

        private bool CheckNeighbor()
        {
            if (gameField.GetLeftChip(selectedChip.Position.x, selectedChip.Position.y) == lastselectedChip && lastselectedChip.State == ChipState.Idle) return true;
            if (gameField.GetRightChip(selectedChip.Position.x, selectedChip.Position.y) == lastselectedChip && lastselectedChip.State == ChipState.Idle) return true;
            if (gameField.GetBottomChip(selectedChip.Position.x, selectedChip.Position.y) == lastselectedChip && lastselectedChip.State == ChipState.Idle) return true;
            if (gameField.GetTopChip(selectedChip.Position.x, selectedChip.Position.y) == lastselectedChip && lastselectedChip.State == ChipState.Idle) return true;
            return false;
        }

        public Vector3 GetMousePosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public void ResetSwap()
        {
            mousePos = Vector3.zero;
            deltaPos = Vector3.zero;
            switchDirection = Vector3.zero;
            selectedChip = null;
            lastselectedChip = null;
            currentlySwapping = false;
        }
        #endregion

        #region Match Handle
        public bool HandleMatches(bool isPlayerMatch)
        {
            var matches = GetMatches();
            inputLocked = true;
            if (matches.Count > 0)
            {
                foreach (var match in matches)
                {
                    CheckMatch(match, isPlayerMatch);
                }
                if (isPlayerMatch) StartCoroutine(ProcessingChips());
                return true;
            }
            else
            {
                if (isPlayerMatch) StartCoroutine(ProcessingChips());
                return false;
            }
        }

        public void CheckMatch(Match match, bool isPlayerMatch)
        {
            var randomChip = match.elements[UnityEngine.Random.Range(0, match.elements.Count)];
            if (isPlayerMatch && match.elements.Contains(lastselectedChip))
            {
                foreach (var chip in match.elements)
                {
                    GetChip(chip.Position.x, chip.Position.y).State = ChipState.Destroy;
                }
                gameField.CreateBonusChip(lastselectedChip.Position.x, lastselectedChip.Position.y, match.matchType);
            }
            else if (isPlayerMatch && match.elements.Contains(selectedChip))
            {
                foreach (var chip in match.elements)
                {
                    GetChip(chip.Position.x, chip.Position.y).State = ChipState.Destroy;
                }
                gameField.CreateBonusChip(selectedChip.Position.x, selectedChip.Position.y, match.matchType);
            }
            else if (!isPlayerMatch)
            {
                foreach (var chip in match.elements)
                {
                    GetChip(chip.Position.x, chip.Position.y).State = ChipState.Destroy;
                }
                gameField.CreateBonusChip(randomChip.Position.x, randomChip.Position.y, match.matchType);
            }
        }
        #endregion

        #region Field Processing
        public void IntitalizeField()
        {
            foreach (var cell in gameField.cells)
            {
                if (cell.chip == null)
                    continue;
                cell.chip.State = ChipState.Appear;
                ChipStateProcessing(cell);
            }
        }
        private bool CheckIdleState()
        {
            foreach (var cell in gameField.cells)
            {
                if (cell.CanChipSet && cell.chip == null || cell.CanChipSet && cell.chip.State != ChipState.Idle)
                {
                    return false;
                }
            }
            inputLocked = false;
            return true;
        }

        [ContextMenu(itemName:"Move")]
        public IEnumerator ProcessingChips()
        {
            if (!CheckIdleState())
            {
                gameField.RespawnChips();
                InitMoveProcessing();
                yield return new WaitForSecondsRealtime(0.12f);
                InitStateProcessing();

                if (!CheckIdleState())
                {
                    HandleMatches(false);
                    StartCoroutine(ProcessingChips());
                    yield break;
                }
            }
        }

        private void InitStateProcessing()
        {
            foreach (var cell in gameField.cells)
            {
                ChipStateProcessing(cell);
            }
        }

        private void InitMoveProcessing()
        {
            foreach (var cell in gameField.cells)
            {
                ChipMoveProcessing(cell);
            }
        }

        public void DestroyChips()
        {
            GetCell(2,2).DestroyChip();
            GetCell(4,5).DestroyChip();
            GetCell(3,4).DestroyChip();
            GetCell(6,2).DestroyChip();
            GetCell(1,4).DestroyChip();
            GetCell(5,6).DestroyChip();
            StartCoroutine(ProcessingChips());
        }
        

        private void ChipMoveProcessing(Cell cellToProcess)
        {
            if (cellToProcess == null)
                return;
            var nextCell = gameField.GetNeighborCell(cellToProcess.Position.x, cellToProcess.Position.y, cellToProcess.Direction);
            if (nextCell == null || cellToProcess.chip == null || nextCell.chip != null)
                return;
            
            GetChip(cellToProcess.Position.x, cellToProcess.Position.y).State = ChipState.Move;
            MoveTo(cellToProcess.chip.transform, nextCell.transform.position, 0.08f, () =>
            {
                nextCell.SetChip(cellToProcess.chip);
                nextCell.chip.State = ChipState.Idle;
                cellToProcess.SetChip(null);
            });
            return;
        }

        private void ChipStateProcessing(Cell cellToProcess)
        {
            if (cellToProcess.chip == null)
                return;
            if (cellToProcess.chip.State == ChipState.Idle)
                return;
            if (cellToProcess.chip.chipBehaviour.IsPlayedAnim)
                return;
            switch (cellToProcess.chip.State)
            {
                case ChipState.Appear:
                    cellToProcess.chip.chipBehaviour.PlayAnim();
                    break;
                case ChipState.BonusAppear:
                    cellToProcess.chip.chipBehaviour.PlayAnim();
                    break;
                case ChipState.Explosion:
                    if (cellToProcess.chip.IsBonus)
                    {
                        GetChip(cellToProcess.Position.x, cellToProcess.Position.x).State = ChipState.Activate;
                        GetChip(cellToProcess.Position.x, cellToProcess.Position.x).activateByExplosion = true;
                        cellToProcess.chip.chipBehaviour.PlayAnim();
                    }
                    else
                    {
                        cellToProcess.chip.chipBehaviour.PlayAnim();
                    }
                    break;
                case ChipState.Activate:
                    cellToProcess.chip.chipBehaviour.PlayAnim();
                    break;
                case ChipState.Destroy:
                    cellToProcess.chip.chipBehaviour.PlayAnim();
                    break;
                case ChipState.Destroyed:
                    cellToProcess.DestroyChip();
                    break;
            }
        }
        #endregion

        public List<Match> GetMatches()
        {
            matchDetector = new MatchDetector();
            return matchDetector.GetMatches();
        }

        private Chip GetChip(int row, int column)
        {
            return gameField.GetChip(row, column);
        }
        private Cell GetCell(int row, int column, bool safe = false)
        {
            return gameField.GetCell(row, column);
        }

        #region Chip Shift
        public void SwapPositions(Transform firstTarget, Transform secondTarget, float time, Action onComplete = null)
        {
            var firstTargetPosition = firstTarget.position;
            var secondTargetPosition = secondTarget.position;
    
            StartCoroutine(Move(firstTarget, secondTargetPosition, time));
            StartCoroutine(Move(secondTarget, firstTargetPosition, time, onComplete));
        }

        public void MoveTo(Transform target, Vector3 destination, float time, Action onComplete = null)
        {
            StartCoroutine(Move(target, destination, time, onComplete));
        }

        public IEnumerator Move(Transform target, Vector3 destination, float time, Action onComplete = null)
        {
            var startTime = Time.time;
            var startPosition = target.position;
            var progress = 0f;

            while (progress < 1)
            {
                progress = (Time.time - startTime) / time;
                var newPosition = Vector3.Lerp(startPosition, destination, progress);

                if (target != null)
                    target.position = newPosition;

                yield return null;
            }

            if (target != null)
                target.position = destination;

            onComplete?.Invoke();
        }
        #endregion
    }
}