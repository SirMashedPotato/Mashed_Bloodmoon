using RimWorld;
using System.Collections.Generic;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class JobDriver_CastAbilityMeleeReserveTarget : JobDriver_CastAbilityMelee
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(TargetIndex.A);
            this.FailOn(() => !job.ability.CanCast && !job.ability.Casting);
            Ability ability = ((Verb_CastAbility)job.verbToUse).ability;
            yield return Toils_Reserve.Reserve(TargetIndex.A);  //Literally the only additional line
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOn(() => !ability.CanApplyOn(job.targetA));
            yield return Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B, canHitNonTargetPawns: false);
        }
    }
}
