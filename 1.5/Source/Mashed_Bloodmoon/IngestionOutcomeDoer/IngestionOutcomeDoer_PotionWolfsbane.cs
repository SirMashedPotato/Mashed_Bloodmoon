using Verse;

namespace Mashed_Bloodmoon
{
    internal class IngestionOutcomeDoer_PotionWolfsbane : IngestionOutcomeDoer_LycanthropePotion
    {
        public override void IngestionOutcome_Dormant(Pawn pawn, Hediff hediff)
        {
            pawn.health.GetOrAddHediff(HediffDefOf.Mashed_Bloodmoon_WolfsbanePrevention).Severity = 1f;
        }

        public override void IngestionOutcome_Human(Pawn pawn)
        {
            pawn.health.GetOrAddHediff(HediffDefOf.Mashed_Bloodmoon_WolfsbaneResistance).Severity = 1f;

        }

        public override void IngestionOutcome_Lycanthrope(Pawn pawn, Hediff hediff)
        {
            pawn.health.GetOrAddHediff(HediffDefOf.Mashed_Bloodmoon_WolfsbaneNausea).Severity = 1f;

            if (Rand.Chance(0.1f))
            {
                ///TODO utility method with letter etc
                pawn.health.RemoveHediff(hediff);
            }
        }

        public override void IngestionOutcome_SaniesLupinus(Pawn pawn, Hediff hediff)
        {
            hediff.Severity -= 0.3f;
        }

        public override void IngestionOutcome_Transformed(Pawn pawn, Hediff hediff)
        {
            pawn.health.GetOrAddHediff(HediffDefOf.Mashed_Bloodmoon_WolfsbaneNausea).Severity = 1f;
            pawn.health.GetOrAddHediff(RimWorld.HediffDefOf.ToxicBuildup);
        }
    }
}
