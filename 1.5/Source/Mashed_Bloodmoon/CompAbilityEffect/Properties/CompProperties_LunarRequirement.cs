using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompProperties_LunarRequirement : CompProperties_AbilityEffect
    {
        public CompProperties_LunarRequirement() => compClass = typeof(CompAbilityEffect_LunarRequirement);

        public List<GameConditionDef> gameConditions;
    }
}