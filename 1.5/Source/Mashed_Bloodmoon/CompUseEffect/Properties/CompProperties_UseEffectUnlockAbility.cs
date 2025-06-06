using Verse;

namespace Mashed_Bloodmoon
{
    public class CompProperties_UseEffectUnlockAbility : CompProperties
    {
        public CompProperties_UseEffectUnlockAbility()
        {
            compClass = typeof(CompUseEffect_UnlockAbility);
        }

        public LycanthropeAbilityDef abilityDef;
    }
}
