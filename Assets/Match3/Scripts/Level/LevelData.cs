using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Match3.Scripts.Levels
{
    /// <summary>
    /// Level data for level editor
    /// </summary>
    [Serializable]  public class LevelData
    {
        public string LevelName;
        public int LevelNumber;
        public int Width;
        public int Height;
        public Field field;
        public LimitType LimitType;
        public int LimitCount;

        public LevelData Clone()
        {
            var other = new LevelData();

            other.LevelName = LevelName;
            other.LevelNumber = LevelNumber;

            other.Width = Width;
            other.Height = Height;
            
            other.field = field.Clone();
            
            other.LimitType = LimitType;
            other.LimitCount = LimitCount;

            return other;
        }
    }
}
