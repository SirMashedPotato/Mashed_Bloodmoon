using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public abstract class LycanthropeAbilityEffectComp : CompAbilityEffect
    {
        public HediffComp_Lycanthrope compLycanthrope;
        public HediffComp_LycanthropeTransformed compLycanthropeTransformed;

        public HediffComp_Lycanthrope CompLycanthrope
        {
            get
            {
                if (compLycanthrope == null)
                {
                    compLycanthrope = LycanthropeUtility.GetCompLycanthrope(parent.pawn);
                }
                return compLycanthrope;
            }
        }

        public HediffComp_LycanthropeTransformed CompLycanthropeTransformed
        {
            get
            {
                if (compLycanthropeTransformed == null)
                {
                    compLycanthropeTransformed = LycanthropeUtility.GetCompLycanthropeTransformed(parent.pawn);
                }
                return compLycanthropeTransformed;
            }
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (!LycanthropeUtility.PawnIsTransformedLycanthrope(parent.pawn))
            {
                reason = "Mashed_Bloodmoon_NotTransformedLycanthrope".Translate(parent.pawn);
                parent.pawn.abilities.RemoveAbility(parent.def);
                return true;
            }

            return base.GizmoDisabled(out reason);
        }
    }
}
