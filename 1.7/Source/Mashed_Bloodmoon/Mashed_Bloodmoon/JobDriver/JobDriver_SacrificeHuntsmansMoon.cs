using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class JobDriver_SacrificeHuntsmansMoon : JobDriver_WolfsbloodAltar
    {
        private CompUsable_WolfsbloodAltarHuntsmansMoon compUsable;

        private CompUsable_WolfsbloodAltarHuntsmansMoon CompUsable
        {
            get
            {
                if (compUsable == null)
                {
                    compUsable = job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable_WolfsbloodAltarHuntsmansMoon>();
                }
                return compUsable;
            }
        }

        public override void Notify_Starting()
        {
            base.Notify_Starting();
            useDuration = CompUsable.Props.useDuration;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            foreach (Toil toil in base.MakeNewToils())
            {
                yield return toil;
            }

            yield return Toils_General.Do(delegate
            {
                Apply(pawn);
            });
        }

        private void Apply(Pawn pawn)
        {
            IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Mashed_Bloodmoon_HuntsmansMoon, Find.World);
            IncidentDefOf.Mashed_Bloodmoon_HuntsmansMoon.Worker.TryExecute(parms);
            SacrificeHearts(pawn, CompUsable.Props.heartCost);
            SacrificeBlood(TargetA.Thing as Building_WolfsbloodAltar, CompUsable.Props.bloodCost);
        }
    }
}
