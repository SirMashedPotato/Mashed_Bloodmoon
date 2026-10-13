using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class JobDriver_WolfsbloodAltarDrinkBlood : JobDriver_WolfsbloodAltar
    {
        public override void Notify_Starting()
        {
            base.Notify_Starting();
            useDuration = job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable_WolfsbloodAltarDrinkBlood>().Props.useDuration;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            foreach (Toil toil in base.MakeNewToils())
            {
                yield return toil;
            }

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
