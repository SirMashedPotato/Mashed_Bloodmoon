using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_HeartCost : LycanthropeAbilityEffectComp
    {
        public new CompProperties_AbilityHeartCost Props => (CompProperties_AbilityHeartCost)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (!Props.multiTarget || target.Pawn == parent.pawn)
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

            if (CompLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts] - (Props.heartCost + TotalHeartCostOfQueuedAbilities()) < 0)
            {
                reason = "Mashed_Bloodmoon_AbilityNotEnoughHearts".Translate(parent.def);
                return true;
            }

            return base.GizmoDisabled(out reason);
        }

        private int TotalHeartCostOfQueuedAbilities()
        {
            int num = 0;

            if (parent.pawn.jobs?.curJob?.verbToUse is Verb_CastAbility verb_CastAbility)
            {
                num += HeartCostOfAbility(verb_CastAbility);
            }

            if (parent.pawn.jobs != null)
            {
                for (int i = 0; i < parent.pawn.jobs.jobQueue.Count; i++)
                {
                    num += HeartCostOfAbility(parent.pawn.jobs.jobQueue[i].job.verbToUse);
                }
            }
            return num;
        }

        private int HeartCostOfAbility(Verb verb)
        {
            if (verb is Verb_CastAbility verb_CastAbility)
            {
                return HeartCostOfAbility(verb_CastAbility);
            }

            return 0;
        }

        private int HeartCostOfAbility(Verb_CastAbility verb_CastAbility)
        {
            CompAbilityEffect_HeartCost compHeartCost = verb_CastAbility.ability.CompOfType<CompAbilityEffect_HeartCost>();
            if (compHeartCost != null)
            {
                return compHeartCost.Props.heartCost;
            }

            return 0;
        }
    }
}
