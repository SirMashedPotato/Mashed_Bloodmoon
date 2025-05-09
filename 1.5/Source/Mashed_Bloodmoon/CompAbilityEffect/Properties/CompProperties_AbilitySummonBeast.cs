using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompProperties_AbilitySummonBeast : CompProperties_AbilityEffect
    {
        public CompProperties_AbilitySummonBeast() => compClass = typeof(CompAbilityEffect_SummonBeast);

        public PawnKindDef pawnKindDef;
        public HediffDef linkedHediff;
        public MentalStateDef stateDef;
        public int count = 1;
        public float radius = 1.9f;
        public LycanthropeBeastHuntDef proficiencyBeastHuntDef;
        public int proficiencyExtraCount = 1;
    }
}
