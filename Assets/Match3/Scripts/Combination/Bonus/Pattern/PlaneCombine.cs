using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using Match3.Scripts.Core;
using UnityEngine;

namespace Match3.Scripts.Combination.Bonus
{
    public class PlaneCombine : CombinePattern
    {
        public override bool CheckPattern(Chip bonusChip, Chip otherChip = null)
        {
            markedChips.Clear();
            if (bonusChip.ChipBonusType != ChipBonusType.Plane)
                return false;
            markedChips = GameField.Instance.GetNeighbors(bonusChip.Position.x, bonusChip.Position.y);
            
            foreach (var chip in markedChips)
            {
                GetChip(chip.Position.x, chip.Position.y).State = ChipState.Explosion;
            }
            GetChip(bonusChip.Position.x, bonusChip.Position.y).State = ChipState.Destroy;
            return true;
        }
    }
}
