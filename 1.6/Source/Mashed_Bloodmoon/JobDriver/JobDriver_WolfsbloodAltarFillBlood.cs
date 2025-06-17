using System.Collections.Generic;
using Verse.AI;
using Verse;

namespace Mashed_Bloodmoon
{
    public class JobDriver_WolfsbloodAltarFillBlood : JobDriver_WolfsbloodAltar
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
                if (altar.CanAddBlood())
                {
                    float bloodlossAmount = altar.AddBlood();

                    Hediff bloodLoss = HediffMaker.MakeHediff(RimWorld.HediffDefOf.BloodLoss, pawn);
                    bloodLoss.Severity = bloodlossAmount;
                    pawn.health.AddHediff(bloodLoss);
                }
            });
        }
    }
}
