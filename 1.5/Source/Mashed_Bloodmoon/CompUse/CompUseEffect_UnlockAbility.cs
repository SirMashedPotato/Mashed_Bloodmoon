using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompUseEffect_UnlockAbility : CompUseEffect
    {
        public CompProperties_UseEffectUnlockAbility Props => (CompProperties_UseEffectUnlockAbility)props;

        public override void PrepareTick()
        {
        }

        public override void DoEffect(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            Props.abilityDef.UnlockAbility(compLycanthrope);
        }

        public override AcceptanceReport CanBeUsedBy(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            if (compLycanthrope == null)
            {
                return "Mashed_Bloodmoon_NotLycanthrope".Translate(pawn);
            }

            if (!Props.abilityDef.CanGainAbility(compLycanthrope))
            {
                return "Mashed_Bloodmoon_AbilityAlreadyUnlocked".Translate(pawn, Props.abilityDef);
            }

            return true;
        }
    }
}
