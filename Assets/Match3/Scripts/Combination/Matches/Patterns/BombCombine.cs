using System.Collections;
using System.Collections.Generic;
using Match3.Scripts.Cells;
using Match3.Scripts.Chips;
using Match3.Scripts.Core;
using UnityEngine;

namespace Match3.Scripts.Combination.Matches
{
    public class BombCombine : CombinePattern
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
                            CheckCombine(currentChip, currentType, Vector2.left);
                            CheckCombine(currentChip, currentType, Vector2.up);
                        }
                    }
                }
            }
            return detectedMatches;
        }

        public override void CheckCombine(Chip chip, ChipType currentType, Vector2 direction)
        {
            CheckTCombine(chip, currentType, direction);
            CheckLCombine(chip, currentType, direction);
        }
        private void CheckTCombine(Chip chip, ChipType currentType, Vector2 direction)
        {
            Vector2 leftDirection = GetDirectionByIndex(GetIndexByDirection(direction)-1);
            Vector2 rightDirection = GetDirectionByIndex(GetIndexByDirection(direction)+1);

            Match match = new Match();
            match.matchType = ChipBonusType.Bomb;

            Chip neighborChip;
            if (chip.ChipType == currentType)
            {
                match.elements.Add(chip);
                chipsCounter = 1;
                while (chipsCounter != 5)
                {
                    neighborChip = GetNeighborChip(chip.Position.x, chip.Position.y, leftDirection);
                    if (neighborChip == null)
                        break;
                    if (!CheckContainsElements(neighborChip))
                        break;
                    if (neighborChip.ChipType == currentType)
                    {
                        match.elements.Add(neighborChip);
                        chipsCounter++;
                    }
                    else
                        break;

                    neighborChip = GetNeighborChip(chip.Position.x, chip.Position.y, rightDirection);
                    if (neighborChip == null)
                        break;
                    if (!CheckContainsElements(neighborChip))
                        break;
                    if (neighborChip.ChipType == currentType)
                    {
                        match.elements.Add(neighborChip);
                        chipsCounter++;
                    }
                    else
                        break;

                    neighborChip = GetNeighborChip(chip.Position.x, chip.Position.y, direction);
                    if (neighborChip == null)
                        break;
                    if (!CheckContainsElements(neighborChip))
                        break;
                    if (neighborChip.ChipType == currentType)
                    {
                        match.elements.Add(neighborChip);
                        chipsCounter++;
                    }
                    else
                        break;

                    neighborChip = GetNeighborChip(neighborChip.Position.x, neighborChip.Position.y, direction);
                    if (neighborChip == null)
                        break;
                    if (!CheckContainsElements(neighborChip))
                        break;
                    if (neighborChip.ChipType == currentType)
                    {
                        match.elements.Add(neighborChip);
                        chipsCounter++;
                    }
                    else
                        break;
                    
                    if (chipsCounter == 5)
                    {
                        detectedMatches.Add(match);
                        return;
                    }
                }
            }
        }


        private void CheckLCombine(Chip chip, ChipType currentType, Vector2 direction)
        {
            Vector2 rightDirection = GetDirectionByIndex(GetIndexByDirection(direction)-1);
            Vector2 downDirection = GetDirectionByIndex(GetIndexByDirection(direction)-1);
            downDirection = GetDirectionByIndex(GetIndexByDirection(downDirection)-1);

            Match match = new Match();
            match.matchType = ChipBonusType.Bomb;

            Chip neighborChip;

            if (chip.ChipType == currentType)
            {
                if (!CheckContainsElements(chip))
                    return;
                match.elements.Add(chip);
                chipsCounter = 1;
                while (chipsCounter != 5)
                {
                    neighborChip = GetNeighborChip(chip.Position.x, chip.Position.y, rightDirection);
                    if (neighborChip == null)
                        break;
                    if (!CheckContainsElements(neighborChip))
                        break;
                    if (neighborChip.ChipType == currentType)
                    {
                        match.elements.Add(neighborChip);
                        chipsCounter++;
                    }
                    else
                        break;

                    neighborChip = GetNeighborChip(neighborChip.Position.x, neighborChip.Position.y, rightDirection);
                    if (neighborChip == null)
                        break;
                    if (!CheckContainsElements(neighborChip))
                        break;
                    if (neighborChip.ChipType == currentType)
                    {
                        match.elements.Add(neighborChip);
                        chipsCounter++;
                    }
                    else
                        break;

                    neighborChip = GetNeighborChip(chip.Position.x, chip.Position.y, downDirection);
                    if (neighborChip == null)
                        break;
                    if (!CheckContainsElements(neighborChip))
                        break;
                    if (neighborChip.ChipType == currentType)
                    {
                        match.elements.Add(neighborChip);
                        chipsCounter++;
                    }
                    else
                        break;

                    neighborChip = GetNeighborChip(neighborChip.Position.x, neighborChip.Position.y, downDirection);
                    if (neighborChip == null)
                        break;
                    if (!CheckContainsElements(neighborChip))
                        break;
                    if (neighborChip.ChipType == currentType)
                    {
                        match.elements.Add(neighborChip);
                        chipsCounter++;
                    }
                    else
                        break;
                    
                    if (chipsCounter == 5)
                    {
                        detectedMatches.Add(match);
                        break;
                    }
                }
            }
        }
    }
}