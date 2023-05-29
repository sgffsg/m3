using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Match3.Scripts.Chips
{
    [Serializable] public class ChipElement
    {
        public string Name;
        public Sprite Sprite;   
        public ChipType ChipType;
        public ChipBonusType ChipBonusType;
    }

    public enum ChipType
    {
        None,
        Chip1,
        Chip2,
        Chip3,
        Chip4,
        Chip5
    }

    public enum ChipBonusType
    {
        None = 0,
        Vertical,
        Horizontal,
        Bomb,
        MultiColor,
        Plane
    }
}
