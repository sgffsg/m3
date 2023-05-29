using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Combination.Bonus;
using Match3.Scripts.Core;
using UnityEngine;
using System;

namespace Match3.Scripts.Chips
{
    public class Chip : MonoBehaviour
    {
        private SpriteRenderer Icon;
        public ChipType ChipType;
        public ChipBonusType ChipBonusType;
        public ChipState State;
        public Vector2Int Position;
        public ChipBehaviour chipBehaviour;
        public ChipBonusType nextBonusType;
        public ChipType nextType;
        public bool CanChipMatch => ChipType != ChipType.None || ChipBonusType != ChipBonusType.None;
        public bool IsBonus => ChipType == ChipType.None && ChipBonusType != ChipBonusType.None;
        public bool activateByExplosion;
        public void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            this.name = $"Chip[{Position.x}][{Position.y}] = {AssetLoader.Instance.chipGraphics.GetElement(ChipType, ChipBonusType).Name}";
            chipBehaviour = GetComponent<ChipBehaviour>();
            InitializeIcon();
        }

        public void InitializeIcon()
        {
            Icon = transform.GetComponentInChildren<SpriteRenderer>();
            Icon.sprite = AssetLoader.Instance.chipGraphics.GetSprite(ChipType, ChipBonusType);
        }
    }
}
