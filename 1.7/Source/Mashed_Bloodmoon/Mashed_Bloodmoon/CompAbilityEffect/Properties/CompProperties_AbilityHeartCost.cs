using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompProperties_AbilityHeartCost : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityHeartCost() => compClass = typeof(CompAbilityEffect_HeartCost);

        public int heartCost = 1;
        public bool multiTarget = false;

        public override IEnumerable<string> ExtraStatSummary()
        {
            yield return "Mashed_Bloodmoon_AbilityHeartCost".Translate(heartCost);
        }
    }
}