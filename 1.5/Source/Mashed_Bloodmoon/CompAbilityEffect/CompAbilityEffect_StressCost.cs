using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_StressCost : LycanthropeAbilityEffectComp
    {
        public new CompProperties_StressCost Props => (CompProperties_StressCost)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            CompLycanthropeTransformed.currentStress += Props.stressCost;
            base.Apply(target, dest);
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (!LycanthropeUtility.PawnIsLycanthrope(parent.pawn))
            {
                reason = "Mashed_Bloodmoon_NotLycanthrope".Translate(parent.pawn);
                return true;
            }
            if (CompLycanthropeTransformed.currentStress + Props.stressCost >= CompLycanthropeTransformed.StressMax)
            {
                reason = "Mashed_Bloodmoon_AbilityStressTooHigh".Translate(parent.def);
                return true;
            }
            return base.GizmoDisabled(out reason);
        }

        public override string ExtraTooltipPart()
        {
            return "Mashed_Bloodmoon_AbilityStressCost".Translate(Props.stressCost);
        }
    }
}
