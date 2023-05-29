using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Match3.Scripts.Chips
{
    [CreateAssetMenu(fileName = "ChipGraphics", menuName = "Match3/Resources/Create ChipGraphics")]
    public class ChipGraphics : ScriptableObject
    {
        [SerializeField]    public List<ChipElement> elements;

        public Sprite GetSprite(ChipType chipType, ChipBonusType chipBonusType)
        {
            foreach (var element in elements)
            {
                if (element.ChipType == chipType && element.ChipBonusType == chipBonusType)
                    return element.Sprite;
            }
            return null;
        }

        public ChipElement GetElement(ChipType chipType, ChipBonusType chipBonusType)
        {
            foreach (var element in elements)
            {
                if (element.ChipType == chipType && element.ChipBonusType == chipBonusType)
                    return element;
            }
            return null;
        }

        public ChipElement GetRandomElement()
        {
            int RandomElementNumber;
            while (true)
            {
                RandomElementNumber = Random.Range(0, elements.Count);
                if (elements[RandomElementNumber].ChipType != ChipType.None && elements[RandomElementNumber].ChipBonusType == ChipBonusType.None)
                    break;
            }
            return elements[RandomElementNumber];
        }
    }
}
