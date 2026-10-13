using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffComp_SaniesLupinus : HediffComp
    {
        public HediffCompProperties_SaniesLupinus Props => (HediffCompProperties_SaniesLupinus)props;

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            if (parent.Severity + severityAdjustment >= 1.0f && !parent.FullyImmune())
            {
                if (LycanthropeUtility.PawnIsDormantLycanthrope(parent.pawn))
                {
                    TransformationUtility.ApplyImminentTransformation(parent.pawn, 300);
                }
                else if (LycanthropeUtility.PawnIsTransformedLycanthrope(parent.pawn))
                {
                    LycanthropeUtility.GetCompLycanthropeTransformed(parent.pawn).StartFury();
                }
                else if (LycanthropeUtility.PawnIsLycanthrope(parent.pawn))
                {
                    LycanthropeUtility.GetCompLycanthrope(parent.pawn).TransformPawn(true);
                }
                else
                {
                    Messages.Message("Mashed_Bloodmoon_SaniesLupinusTransitioned".Translate(parent.pawn), parent.pawn, MessageTypeDefOf.ThreatSmall);
                    parent.pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeDormant);
                }

                parent.pawn.health.RemoveHediff(parent);
                return;
            }

            if (parent.FullyImmune())
            {
                parent.pawn.health.RemoveHediff(parent);
                return;
            }

            base.CompPostTickInterval(ref severityAdjustment, delta);
        }
    }
}
