using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    internal class IngestionOutcomeDoer_PotionWolfsbane : IngestionOutcomeDoer_LycanthropePotion
    {
        public override void IngestionOutcome_Dormant(Pawn pawn, Hediff hediff)
        {
            pawn.health.GetOrAddHediff(HediffDefOf.Mashed_Bloodmoon_WolfsbanePrevention);
        }

        public override void IngestionOutcome_Human(Pawn pawn)
        {
            pawn.health.GetOrAddHediff(HediffDefOf.Mashed_Bloodmoon_WolfsbaneResistance);

        }

        public override void IngestionOutcome_Lycanthrope(Pawn pawn, Hediff hediff)
        {
            pawn.health.GetOrAddHediff(HediffDefOf.Mashed_Bloodmoon_WolfsbaneNausea);

            if (Rand.Chance(0.1f))
            {
                Messages.Message("Mashed_Bloodmoon_LycanthropeCured".Translate(pawn), pawn, MessageTypeDefOf.PositiveEvent);
                pawn.health.RemoveHediff(hediff);
            }
        }

        public override void IngestionOutcome_SaniesLupinus(Pawn pawn, Hediff hediff)
        {
            hediff.Severity -= 0.3f;
        }

        public override void IngestionOutcome_Transformed(Pawn pawn, Hediff hediff)
        {
            pawn.health.GetOrAddHediff(HediffDefOf.Mashed_Bloodmoon_WolfsbaneNausea);
            LycanthropeUtility.LycanthropeIngestedWolfsbane(pawn);
        }
    }
}
