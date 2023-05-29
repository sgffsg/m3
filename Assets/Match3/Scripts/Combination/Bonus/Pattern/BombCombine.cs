using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using Match3.Scripts.Core;
using UnityEngine;

namespace Match3.Scripts.Combination.Bonus
{
    public class BombCombine : CombinePattern
    {
        public override bool CheckPattern(Chip bonusChip, Chip otherChip = null)
        {
            markedChips.Clear();
            if (bonusChip.ChipBonusType != ChipBonusType.Bomb)
                return false;
            FillArrays(GetCell(bonusChip.Position.x, bonusChip.Position.y));
            
            foreach (var chip in markedChips)
            {
                GetChip(chip.Position.x, chip.Position.y).State = ChipState.Explosion;
            }
            GetChip(bonusChip.Position.x, bonusChip.Position.y).State = ChipState.Destroy;
            return true;
        }

        private void FillArrays(Cell bonusChipCell)
        {
            foreach (var chip in GameField.Instance.GetChipsAroundFirst(bonusChipCell.Position.x, bonusChipCell.Position.y))
                markedChips.Add(chip);
        }
    }
}
