using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using Match3.Scripts.Core;
using UnityEngine;

namespace Match3.Scripts.Combination.MultipleBonus
{
    public class MultipleBonusCombiner
    {
        private CombinePattern pattern;

        public bool Check(Chip bonusChip1, Chip bonusChip2 = null)
        {
            if (!bonusChip1.IsBonus || !bonusChip2.IsBonus)
            {
                return false;
            }
            
            Debug.Log(bonusChip1.name + "/" + bonusChip2.name);
            /* pattern = new MultiColorCombine();
            if (pattern.CheckPattern(bonusChip, otherChip))
                return true;

            pattern = new BombCombine();
            if (pattern.CheckPattern(bonusChip, otherChip))
                return true;

            pattern = new PlaneCombine();
            if (pattern.CheckPattern(bonusChip, otherChip))
                return true;

            pattern = new Horizontal();
            if (pattern.CheckPattern(bonusChip, otherChip))
                return true;

            pattern = new VerticalCombine();
            if (pattern.CheckPattern(bonusChip, otherChip))
                return true;
 */
            return false;
            //FieldHandler.Instance.FieldState = FieldState.ProcessingChips;
        }
    }
}
