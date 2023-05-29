using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using Match3.Scripts.Core;
using UnityEngine;

namespace Match3.Scripts.Combination.MultipleBonus
{
    public class CombinePattern
    {
        public GameField gameField;
        public List<Chip> markedChips;

        public CombinePattern()
        {
            gameField = GameField.Instance;
            markedChips = new List<Chip>();
        }

        public virtual bool CheckPattern(Chip bonusChip, Chip otherChip = null)
        {
            return false;
        }

        public Cell GetCell(int row, int column)
        {
            return gameField.GetCell(row, column);
        }
        public Chip GetChip(int row, int column)
        {
            return gameField.GetChip(row, column);
        }
        public Cell GetNeighborCell(int row, int column, Vector2 direction)
        {
            return gameField.GetNeighborCell(row, column, direction);
        }
        public Chip GetNeighborChip(int row, int column, Vector2 direction)
        {
            return gameField.GetNeighborChip(row, column, direction);
        }
    }
}
