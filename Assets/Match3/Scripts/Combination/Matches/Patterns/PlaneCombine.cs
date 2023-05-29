using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using Match3.Scripts.Core;
using UnityEngine;

namespace Match3.Scripts.Combination.Matches
{
    public class PlaneCombine : CombinePattern
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
                            CheckCombine(currentChip, currentType, Vector2.up);
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

            int directionIndex = GetIndexByDirection(direction);
            directionIndex = (int)Mathf.Repeat(directionIndex + 1, 4);
            direction = GetDirectionByIndex(directionIndex);
            
            Chip neighborChip = GetNeighborChip(chip.Position.x, chip.Position.y, direction);
            Match match = new Match();
            match.matchType = ChipBonusType.Plane;

            if (chip.ChipType == currentType)
            {
                if (!CheckContainsElements(chip))
                    return;
                match.elements.Add(chip);
                chipsCounter = 1;

                while (direction != Vector2.up)
                {
                    if (neighborChip == null)
                        break;
                    if (!CheckContainsElements(neighborChip))
                        break;
                    if (neighborChip.ChipType == currentType)
                    {
                        chipsCounter++;
                        match.elements.Add(neighborChip);

                        directionIndex = (int)Mathf.Repeat(directionIndex + 1, 4);
                        direction = GetDirectionByIndex(directionIndex);
                        neighborChip = GetNeighborChip(neighborChip.Position.x, neighborChip.Position.y, direction);

                        if (chipsCounter == 4)
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
