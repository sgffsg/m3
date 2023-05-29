using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Match3.Scripts.Cells
{
    [Serializable] public class CellElement
    {
        public string Name;
        public Sprite Sprite;   
        public CellType CellType;
        public BlockerType BlockerType;
    }

    public enum CellType
    {
        None = 0,
        EmptyBlock
    }

    public enum BlockerType
    {
        None = 0,
        Box,
        Wire
    }
}
