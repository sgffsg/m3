using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using UnityEngine;
using System;

namespace Match3.Scripts.Editor
{
    [Serializable]  public class CellForEditor
    {
        public CellElement cellElement;

        public CellForEditor(CellElement cellElement)
        {
            this.cellElement = cellElement;
        }
    }

    public class CellBlockSelected
    {
        public CellType cellType;
        public BlockerType blockerType;
        public Vector2 direction;
        public bool IsEnterPoint;
        public CellBlockSelected(CellType type, BlockerType blocker)
        {
            cellType = type;
            blockerType = blocker;
        }
        public CellBlockSelected(CellType type, BlockerType blocker, Vector2 direct)
        {
            cellType = type;
            blockerType = blocker;
            direction = direct;
        }
    }
}