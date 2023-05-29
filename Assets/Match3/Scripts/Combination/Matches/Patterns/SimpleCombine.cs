using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using Match3.Scripts.Core;
using UnityEngine;

namespace Match3.Scripts.Combination.Matches
{
    public class SimpleCombine : CombinePattern
    {
        private int chipsCounter = 0;
        public override List<Match> CheckPattern()
        {
            detectedMatches = new List<Match>();
            foreach (var currentType in chipsToCheck)
            {
                for (int row = 0; row < GameField.Instance.levelData.Height; row++)
                {
                    for (int column = 0; column < GameField.Instance.levelData.Width; column++)
                    {
                        Chip currentChip = GetChip(row, column);
                        if (currentChip != null)
                        {
                            CheckCombine(currentChip, currentType, Vector2.right);
                            CheckCombine(currentChip, currentType, Vector2.down);
                        }
                    }
                }
            }
            return detectedMatches;
        }

        public override void CheckCombine(Chip chip, ChipType currentType, Vector2 direction)
        {
            if (chip == null)
                return;
            Chip neighborChip = GetNeighborChip(chip.Position.x, chip.Position.y, direction);
            if (neighborChip == null)
                return;

            Match match = new Match();
            match.matchType = ChipBonusType.None;
            
            if (chip.ChipType == currentType)
            {
                if (!CheckContainsElements(chip))
                    return;

                match.elements.Add(chip);
                chipsCounter = 1;

                while (chipsCounter != 3)
                {
                    if (neighborChip == null)
                        break;

                    if (!CheckContainsElements(neighborChip))
                        break;
                    if (neighborChip.ChipType == currentType)
                    {
                        match.elements.Add(neighborChip);
                        chipsCounter++;

                        neighborChip = GetNeighborChip(neighborChip.Position.x, neighborChip.Position.y, direction);
                        if (chipsCounter == 3)
                        {
                            detectedMatches.Add(match);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
