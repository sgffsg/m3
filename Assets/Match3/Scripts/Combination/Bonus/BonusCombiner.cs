using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using Match3.Scripts.Core;
using UnityEngine;

namespace Match3.Scripts.Combination.Bonus
{
    public class BonusCombiner
    {
        private CombinePattern pattern;

        public bool Check(Chip chip1, Chip chip2 = null)
        {
            var bonusChip = chip1.IsBonus ? chip1 : chip2;
            var otherChip = !chip1.IsBonus ? chip1 : chip2;
            //Debug.Log(otherChip.name);
            //return false; 
            if (!bonusChip.IsBonus || otherChip.IsBonus)
            {
                return false;
            }

            pattern = new MultiColorCombine();
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

            return false;
        }
    }
}
