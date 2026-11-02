using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_FeralAutoUse : CompAbilityEffect
    {
        private new CompProperties_AbilityFeralAutoUse Props => (CompProperties_AbilityFeralAutoUse)props;

        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            if (Props.onlyInCombat && !parent.pawn.mindState.CombatantRecently)
            {
                return false;
            }

            if (Props.mustBeHostile && !parent.pawn.HostileTo(target.Thing))
            {
                return false;
            }

            if (!Props.canTargetAnimals && target.Thing is Pawn { IsAnimal: true })
            {
                return false;
            }
            
            if (Props.minPain > 0f && parent.pawn.health.hediffSet.PainTotal < Props.minPain)
            {
                return false;
            }
            
            if (Props.minBleedRate > 0f && parent.pawn.health.hediffSet.BleedRateTotal < Props.minBleedRate)
            {
                return false;
            }
            
            return base.AICanTargetNow(target);
        }
    }
}