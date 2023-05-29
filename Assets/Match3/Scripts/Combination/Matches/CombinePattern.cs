using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using Match3.Scripts.Core;
using UnityEngine;

namespace Match3.Scripts.Combination.Matches
{
    public class CombinePattern
    {
        public ChipType[] chipsToCheck = {ChipType.Chip1, ChipType.Chip2, ChipType.Chip3, ChipType.Chip4, ChipType.Chip5};
        public List<Match> detectedMatches;
        public virtual List<Match> CheckPattern()
        {
            return detectedMatches;
        }

        public virtual void CheckCombine(Chip chip, ChipType currentType, Vector2 direction)
        {
            return;
        }

        public bool CheckContainsElements(Chip chip)
        {
            foreach (var match in detectedMatches)
            {
                if (match.elements.Contains(chip))
                {
                    return false;
                }
            }
            return true;
        }

        public Vector2 GetDirectionByIndex(int index)
        {
            if (index == 0) return Vector2.up;
            if (index == 1) return Vector2.right;
            if (index == 2) return Vector2.down;
            if (index == 3) return Vector2.left;
            if (index < 0) return Vector2.left;
            return Vector2.up;
        }
        public int GetIndexByDirection(Vector2 direction)
        {
            if (direction == Vector2.left) return 3;
            if (direction == Vector2.down) return 2;
            if (direction == Vector2.right) return 1;
            return 0;
        }

        
        public Cell GetCell(int row, int column)
        {
            return GameField.Instance.GetCell(row, column);
        }
        public Chip GetChip(int row, int column)
        {
            return GameField.Instance.GetChip(row, column);
        }
        public Cell GetNeighborCell(int row, int column, Vector2 direction)
        {
            return GameField.Instance.GetNeighborCell(row, column, direction);
        }
        public Chip GetNeighborChip(int row, int column, Vector2 direction)
        {
            return GameField.Instance.GetNeighborChip(row, column, direction);
        }
    }
}
