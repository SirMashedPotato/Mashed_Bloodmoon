using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompProperties_AbilityLunarRequirement : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityLunarRequirement() => compClass = typeof(CompAbilityEffect_LunarRequirement);

        public List<GameConditionDef> gameConditions;
        public LycanthropeBeastHuntDef beastHuntDef;
    }
}