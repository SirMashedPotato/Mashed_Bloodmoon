using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompProperties_AbilityProficiency : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityProficiency() => compClass = typeof(CompAbilityEffect_AbilityProficiency);

        public LycanthropeBeastHuntDef beastHuntDef;
        public bool incrementBeastHunt = false;
        [MustTranslate]
        public string completedTooltip;
    }
}