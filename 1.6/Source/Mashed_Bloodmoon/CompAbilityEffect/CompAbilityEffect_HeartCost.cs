using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_HeartCost : LycanthropeAbilityEffectComp
    {
        public new CompProperties_AbilityHeartCost Props => (CompProperties_AbilityHeartCost)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.Pawn == parent.pawn)
            {
                CompLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts] -= Props.heartCost;
            }
            base.Apply(target, dest);
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (CompLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts] - Props.heartCost < 0)
            {
                reason = "Mashed_Bloodmoon_AbilityNotEnoughHearts".Translate(parent.def);
                return true;
            }
            return base.GizmoDisabled(out reason);
        }

        public override string ExtraTooltipPart()
        {
            return "Mashed_Bloodmoon_AbilityHeartCost".Translate(Props.heartCost);
        }
    }
}
