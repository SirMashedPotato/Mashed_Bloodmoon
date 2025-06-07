using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class JobDriver_WolfsbloodAltarDrinkBlood : JobDriver
    {
        private int useDuration = -1;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref useDuration, "useDuration", 0);
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }

        public override void Notify_Starting()
        {
            base.Notify_Starting();
            useDuration = job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable_WolfsbloodAltarDrinkBlood>().Props.useDuration;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);

            Toil toiLWait = Toils_General.Wait(useDuration, TargetIndex.A);
            toiLWait.WithProgressBarToilDelay(TargetIndex.A);
            yield return toiLWait;

            yield return Toils_General.Do(delegate
            {
                Building_WolfsbloodAltar altar = TargetA.Thing as Building_WolfsbloodAltar;
                if (altar.CanConsumeBlood())
                {
                    altar.ConsumeBlood();

                    IngestionOutcomeDoer_PotionWolfsblood ingestionOutcomeDoer = new IngestionOutcomeDoer_PotionWolfsblood();
                    ingestionOutcomeDoer.DoIngestionOutcome(pawn, altar, 1);
                }
            });
        }
    }
}
