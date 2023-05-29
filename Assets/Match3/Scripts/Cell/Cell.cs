using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Match3.Scripts.Core;
using Match3.Scripts.Chips;
using UnityEngine;
using System;

namespace Match3.Scripts.Cells
{
    public class Cell : MonoBehaviour
    {
        private SpriteRenderer CellSprite;
        private SpriteRenderer BlockerSprite;

        public CellType CellType;
        public BlockerType CellBlockerType;
        public Vector2 Direction;
        public bool IsEnterPoint;
        public Vector2Int Position;
        public Chip chip;
        public bool CanChipSet => CellType != CellType.None;
        public bool CanChipMove => CellBlockerType == BlockerType.None && chip.CanChipMatch;


        public void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            this.name = $"Cell[{Position.x}][{Position.y}] = {AssetLoader.Instance.cellGraphics.GetElement(CellType, CellBlockerType).Name}";
            InitializeIcons();
        }

        public void InitializeIcons()
        {
            CellSprite = transform.Find("CellSprite").GetComponent<SpriteRenderer>();
            BlockerSprite = transform.Find("BlockerSprite").GetComponent<SpriteRenderer>();
            CellSprite.gameObject.SetActive(false);
            BlockerSprite.gameObject.SetActive(false);

            if (CellType != CellType.None)
            {
                CellSprite.gameObject.SetActive(true);
                CellSprite.sprite = AssetLoader.Instance.cellGraphics.GetSprite(CellType.EmptyBlock, BlockerType.None);
            }

            if (CellBlockerType != BlockerType.None)
            {
                BlockerSprite.gameObject.SetActive(true);
                BlockerSprite.sprite = AssetLoader.Instance.cellGraphics.GetSprite(CellType, CellBlockerType);
            }
        }

        public void SetChip(Chip chipToSet)
        {
            if (chipToSet == null)
            {
                chip = null;
                return;
            }
            chip = chipToSet;
            chip.Position = Position;
            chip.Initialize();
        }

        public void RemoveBlocker()
        {
            CellBlockerType = BlockerType.None;
            Initialize();
        }

        public void DestroyChip()
        {
            if (chip == null)
                return;

            Destroy(chip.gameObject);
            chip= null;
        }

        public void DestroyCell()
        {
            Destroy(this.gameObject);
        }
    }
}
