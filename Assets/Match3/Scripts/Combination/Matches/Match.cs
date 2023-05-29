using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using UnityEngine;

namespace Match3.Scripts.Combination.Matches
{
    public class Match
    {
        public ChipBonusType matchType;
        public List<Chip> elements = new List<Chip>();
    }
}
