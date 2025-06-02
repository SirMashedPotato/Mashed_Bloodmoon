using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompProperties_AbilityTendWounds : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityTendWounds() => compClass = typeof(CompAbilityEffect_AbilityTendWounds);

        public int tendCount = 3;
        public FloatRange tendQualityRange = FloatRange.ZeroToOne;
    }
}