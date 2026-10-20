using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class JobDriver_SacrificeFeralWerewolfRaid : JobDriver_WolfsbloodAltar
    {
        private CompUsable_WolfsbloodAltarFeralWerewolfRaid compUsable;

        private CompUsable_WolfsbloodAltarFeralWerewolfRaid CompUsable
        {
            get
            {
                if (compUsable == null)
                {
                    compUsable = job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable_WolfsbloodAltarFeralWerewolfRaid>();
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
            WerewolfUtility.TriggerWerewolfRaid(pawn.Map);
            SacrificeHearts(pawn, CompUsable.Props.heartCost);
            SacrificeBlood(TargetA.Thing as Building_WolfsbloodAltar, CompUsable.Props.bloodCost);
        }
    }
}
