using RimWorld;

namespace Mashed_Bloodmoon
{
    public class CompProperties_AbilityConsumeHeart : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityConsumeHeart() => compClass = typeof(CompAbilityEffect_ConsumeHeart);

        public float nutritionFactor = 0.01f;
    }
}