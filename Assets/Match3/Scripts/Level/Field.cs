using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using UnityEngine;
using System;

namespace Match3.Scripts.Levels
{
    [Serializable] public class Field
    {
        public Cell[] cells = new Cell[0];
        public Chip[] chips = new Chip[0];

        public Field Clone()
        {
            var other = new Field();
            other.cells = new Cell[cells.Length];
            other.chips = new Chip[chips.Length];

            for (int i = 0; i < cells.Length; i++)
                other.cells[i] = cells[i].Clone();

            for (int i = 0; i < chips.Length; i++)
                other.chips[i] = chips[i].Clone();

            return other;
        }


        [Serializable] public class Chip
        {
            public ChipType chipType;
            public ChipBonusType chipBonusType;
            
            public Chip(ChipType Type, ChipBonusType BonusType)
            {
                this.chipType = Type;
                this.chipBonusType = BonusType;
            }

            public Chip Clone()
            {
                var other = new Chip(chipType, chipBonusType);

                return other;
            }
        }

        [Serializable] public class Cell
        {
            public CellType cellType;
            public BlockerType blockerType;
            public Vector2 direction;
            public bool IsEnterPoint;
            public Cell(CellType Type, BlockerType BlockType, Vector2 Direction, bool IsEnter)
            {
                this.cellType = Type;
                this.blockerType = BlockType;
                this.direction = Direction;
                this.IsEnterPoint = IsEnter;
            }

            public Cell Clone()
            {
                var other = new Cell(cellType, blockerType, direction, IsEnterPoint);

                return other;
            }
        }
    }

    
}
