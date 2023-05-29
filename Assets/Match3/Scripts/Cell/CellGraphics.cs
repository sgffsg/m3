using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Match3.Scripts.Cells
{
    [CreateAssetMenu(fileName = "CellGraphics", menuName = "Match3/Resources/Create CellGraphics")]
    public class CellGraphics : ScriptableObject
    {
        [SerializeField]    public List<CellElement> elements;

        public Sprite GetSprite(CellType cellType, BlockerType blockerType)
        {
            foreach (var element in elements)
            {
                if (element.CellType == cellType && element.BlockerType == blockerType)
                    return element.Sprite;
            }
            return null;
        }

        public CellElement GetElement(CellType cellType, BlockerType blockerType)
        {
            foreach (var element in elements)
            {
                if (element.CellType == cellType && element.BlockerType == blockerType)
                    return element;
            }
            return null;
        }

    }
}
