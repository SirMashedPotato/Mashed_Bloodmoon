using System.Collections.Generic;
using Verse.AI;
using Verse;

namespace Mashed_Bloodmoon
{
    public abstract class JobDriver_WolfsbloodAltar : JobDriver
    {
        public int useDuration = -1;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref useDuration, "useDuration", 0);
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);

            Toil toilWait = Toils_General.Wait(useDuration, TargetIndex.A);
            toilWait.WithProgressBarToilDelay(TargetIndex.A);
            yield return toilWait;
        }
    }
}
