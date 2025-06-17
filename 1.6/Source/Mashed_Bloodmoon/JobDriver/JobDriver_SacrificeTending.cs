using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class JobDriver_SacrificeTending : JobDriver_WolfsbloodAltar
    {
        private const int tendCount = 5;
        private static FloatRange tendQualityRange = new FloatRange(0.4f, 0.6f);

        private CompUsable_WolfsbloodAltarTendWounds compUsable;

        private CompUsable_WolfsbloodAltarTendWounds CompUsable
        {
            get
            {
                if(compUsable == null)
                {
                    compUsable = job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable_WolfsbloodAltarTendWounds>();
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
                if (AbilityUtility.ValidateHasTendableWound(pawn, false, null))
                {
                    Apply(pawn);
                }
            });
        }

        private void Apply(Pawn pawn)
        {
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs.Where(x => x.TendableNow() && (x is Hediff_Injury || x is Hediff_MissingPart)).InRandomOrder().ToList();
            if (hediffs.NullOrEmpty())
            {
                return;
            }

            int finalTendCount = Math.Min(tendCount, hediffs.Count);
            if (finalTendCount <= 0)
            {
                return;
            }

            for (int i = 0; i < finalTendCount; i++)
            {
                hediffs[i].Tended(tendQualityRange.RandomInRange, tendQualityRange.TrueMax, 1);
            }

            SacrificeHearts(pawn, CompUsable.Props.heartCost);

            MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "NumWoundsTended".Translate(finalTendCount), 3.65f);
        }
    }
}
