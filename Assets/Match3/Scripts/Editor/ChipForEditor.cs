using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using UnityEngine;
using System;

namespace Match3.Scripts.Editor
{
    [Serializable]  public class ChipForEditor
    {
        public ChipElement chipElement;

        public ChipForEditor(ChipElement chipElement)
        {
            this.chipElement = chipElement;
        }
    }

    
    public class ChipBlockSelected
    {
        public ChipType chipType;
        public ChipBonusType chipBonusType;
        public ChipBlockSelected(ChipType type, ChipBonusType bonus)
        {
            chipType = type;
            chipBonusType = bonus;
        }
    }
}