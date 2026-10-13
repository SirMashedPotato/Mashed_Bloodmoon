using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompProperties_AbilityStressCost : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityStressCost() => compClass = typeof(CompAbilityEffect_StressCost);

        public int stressCost = 1;
        public bool multiTarget = false;

        public override IEnumerable<string> ExtraStatSummary()
        {
            yield return "Mashed_Bloodmoon_AbilityStressCost".Translate(stressCost);
        }
    }
}