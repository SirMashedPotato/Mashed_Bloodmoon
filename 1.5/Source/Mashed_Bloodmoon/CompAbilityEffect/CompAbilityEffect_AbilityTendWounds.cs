using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_AbilityTendWounds : CompAbilityEffect
    {
        public new CompProperties_AbilityTendWounds Props => (CompProperties_AbilityTendWounds)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            List<Hediff> hediffs = parent.pawn.health.hediffSet.hediffs.Where(x=>x.TendableNow() && (x is Hediff_Injury || x is Hediff_MissingPart)).InRandomOrder().ToList();
            if (hediffs.NullOrEmpty())
            {
                return;
            }

            int finalTendCount = Math.Min(Props.tendCount, hediffs.Count);
            if (finalTendCount <= 0)
            {
                return;
            }

            for (int i = 0; i < finalTendCount; i++)
            {
                hediffs[i].Tended(Props.tendQualityRange.RandomInRange, Props.tendQualityRange.TrueMax, 1);
            }

            MoteMaker.ThrowText(parent.pawn.DrawPos, parent.pawn.Map, "NumWoundsTended".Translate(finalTendCount), 3.65f);
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (!AbilityUtility.ValidateHasTendableWound(parent.pawn, false, parent))
            {
                reason = "AbilityMustHaveTendableWound".Translate(parent.pawn);
                return true;
            }
            return base.GizmoDisabled(out reason);
        }
    }
}