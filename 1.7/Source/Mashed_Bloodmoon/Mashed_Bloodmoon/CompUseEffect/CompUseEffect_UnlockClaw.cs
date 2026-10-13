using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompUseEffect_UnlockClaw : CompUseEffect
    {
        public CompProperties_UseEffectUnlockClaw Props => (CompProperties_UseEffectUnlockClaw)props;

        public override void PrepareTick()
        {
        }

        public override void DoEffect(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            Props.clawDef.Unlock(compLycanthrope);
        }

        public override AcceptanceReport CanBeUsedBy(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            if (compLycanthrope == null)
            {
                return "Mashed_Bloodmoon_NotLycanthrope".Translate(pawn);
            }

            if (Props.clawDef.AlreadyUnlocked(compLycanthrope))
            {
                return "Mashed_Bloodmoon_ClawAlreadyUnlocked".Translate(pawn, Props.clawDef);
            }

            return true;
        }
    }
}
