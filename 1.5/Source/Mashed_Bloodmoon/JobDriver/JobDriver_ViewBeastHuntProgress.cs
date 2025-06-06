using System.Collections.Generic;
using Verse.AI;
using Verse;

namespace Mashed_Bloodmoon
{
    public class JobDriver_ViewBeastHuntProgress : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
            yield return Toils_General.Do(delegate
            {
                Page_BeastHuntProgress page = new Page_BeastHuntProgress(LycanthropeUtility.GetCompLycanthrope(pawn));
                Find.WindowStack.Add(page);
            });
        }
    }
}
