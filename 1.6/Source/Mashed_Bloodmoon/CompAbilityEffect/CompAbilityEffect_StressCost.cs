using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_StressCost : LycanthropeAbilityEffectComp
    {
        public new CompProperties_AbilityStressCost Props => (CompProperties_AbilityStressCost)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (!Props.multiTarget || target.Pawn == parent.pawn)
            {
                CompLycanthropeTransformed.currentStress += Props.stressCost;
            }
            base.Apply(target, dest);
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (CompLycanthropeTransformed.currentStress + Props.stressCost >= CompLycanthropeTransformed.StressMax)
            {
                reason = "Mashed_Bloodmoon_AbilityStressTooHigh".Translate(parent.def);
                return true;
            }
            return base.GizmoDisabled(out reason);
        }
    }
}
