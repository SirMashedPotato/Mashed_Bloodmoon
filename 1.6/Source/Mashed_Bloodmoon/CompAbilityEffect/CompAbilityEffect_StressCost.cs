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

            if (CompLycanthropeTransformed.currentStress + Props.stressCost + TotalStressCostOfQueuedAbilities() >= CompLycanthropeTransformed.StressMax)
            {
                reason = "Mashed_Bloodmoon_AbilityStressTooHigh".Translate(parent.def);
                return true;
            }

            return base.GizmoDisabled(out reason);
        }

        private int TotalStressCostOfQueuedAbilities()
        {
            int num = 0;

            if (parent.pawn.jobs?.curJob?.verbToUse is Verb_CastAbility verb_CastAbility)
            {
                num += StressCostOfAbility(verb_CastAbility);
            }

            if (parent.pawn.jobs != null)
            {
                for (int i = 0; i < parent.pawn.jobs.jobQueue.Count; i++)
                {
                    num += StressCostOfAbility(parent.pawn.jobs.jobQueue[i].job.verbToUse);
                }
            }
            Log.Message("queued stress cost: " + num);
            return num;
        }

        private int StressCostOfAbility(Verb verb)
        {
            if (verb is Verb_CastAbility verb_CastAbility)
            {
                return StressCostOfAbility(verb_CastAbility);
            }

            return 0;
        }

        private int StressCostOfAbility(Verb_CastAbility verb_CastAbility)
        {
            CompAbilityEffect_StressCost compStressCost = verb_CastAbility.ability.CompOfType<CompAbilityEffect_StressCost>();
            if (compStressCost != null)
            {
                return compStressCost.Props.stressCost;
            }

            return 0;
        }
    }
}
