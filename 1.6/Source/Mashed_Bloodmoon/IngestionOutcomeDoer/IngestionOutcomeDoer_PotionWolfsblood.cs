using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class IngestionOutcomeDoer_PotionWolfsblood : IngestionOutcomeDoer_LycanthropePotion
    {
        public override void IngestionOutcome_Dormant(Pawn pawn, Hediff hediff)
        {
            TransformationUtility.ApplyImminentTransformation(pawn, 300);
        }

        public override void IngestionOutcome_Human(Pawn pawn)
        {
            if (pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropyResistance) < 1)
            {
                pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_SaniesLupinus).Severity = 0.1f;
            }
        }

        public override void IngestionOutcome_Lycanthrope(Pawn pawn, Hediff hediff)
        {
            pawn.health.GetOrAddHediff(HediffDefOf.Mashed_Bloodmoon_WolfsbloodAdrenaline);
        }

        public override void IngestionOutcome_SaniesLupinus(Pawn pawn, Hediff hediff)
        {
            hediff.Severity += 0.3f;
        }

        public override void IngestionOutcome_Transformed(Pawn pawn, Hediff hediff)
        {
            LycanthropeUtility.GetCompLycanthropeTransformed(pawn).AddLinkedHediff(HediffDefOf.Mashed_Bloodmoon_WolfsbloodRegeneration);
        }
    }
}
