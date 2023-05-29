using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts.Combination.Matches
{
    public class MatchDetector
    {
        private CombinePattern pattern;
        public List<Match> matches;

        public List<Match> GetMatches()
        {
            matches = new List<Match>();

            pattern = new MultiColorCombine();
            AddCombinationsToMatchList(pattern.CheckPattern());

            pattern = new BombCombine();
            AddCombinationsToMatchList(pattern.CheckPattern());

            pattern = new PlaneCombine();
            AddCombinationsToMatchList(pattern.CheckPattern());

            pattern = new HorizontalCombine();
            AddCombinationsToMatchList(pattern.CheckPattern());

            pattern = new VerticalCombine();
            AddCombinationsToMatchList(pattern.CheckPattern());

            pattern = new SimpleCombine();
            AddCombinationsToMatchList(pattern.CheckPattern());

            //PrintFoundMatches();
            return matches;
        }

        public void PrintFoundMatches()
        {
            Debug.Log($"Combines found {matches.Count}");
            foreach(var match in matches)
            {
                string Comb = $"CombinationType: {match.matchType}\n";
                foreach(var matchChip in match.elements)
                {
                    Comb+=$"{matchChip.name}\n";
                }
                Debug.Log(Comb);
            }
        }

        private void AddCombinationsToMatchList(List<Match> matchesToAdd)
        {
            foreach(var match in matchesToAdd)
            {
                if (CheckContainsElements(match))
                    matches.Add(match);
            }
        }


        private bool CheckContainsElements(Match matchToCheck)
        {
            foreach (var match in matches)
            {
                foreach (var matchChip in matchToCheck.elements)
                {
                    if (match.elements.Contains(matchChip))
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
    }
}
