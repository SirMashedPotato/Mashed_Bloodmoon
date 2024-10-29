using RimWorld;

namespace Mashed_Bloodmoon
{
    public class CompProperties_ConsumeHeart : CompProperties_AbilityEffect
    {
        public CompProperties_ConsumeHeart() => compClass = typeof(CompAbilityEffect_ConsumeHeart);

        public float nutritionFactor = 0.01f;
    }
}