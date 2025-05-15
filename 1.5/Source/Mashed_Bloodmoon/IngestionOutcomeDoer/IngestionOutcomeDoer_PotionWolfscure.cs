using RimWorld;
using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class IngestionOutcomeDoer_PotionWolfscure : IngestionOutcomeDoer_LycanthropePotion
    {
        public override void IngestionOutcome_Dormant(Pawn pawn, Hediff hediff)
        {
        }

        public override void IngestionOutcome_Human(Pawn pawn)
        {
        }

        public override void IngestionOutcome_Lycanthrope(Pawn pawn, Hediff hediff)
        {
            pawn.health.RemoveHediff(hediff);
            pawn.jobs.StartJob(new Job(JobDefOf.Vomit));
            Messages.Message("Mashed_Bloodmoon_LycanthropeCured".Translate(pawn), pawn, MessageTypeDefOf.PositiveEvent);
            //todo spawn a thing that spawns a spectral werewolf
        }

        public override void IngestionOutcome_SaniesLupinus(Pawn pawn, Hediff hediff)
        {
        }

        public override void IngestionOutcome_Transformed(Pawn pawn, Hediff hediff)
        {
            pawn.health.RemoveHediff(hediff);
            IngestionOutcome_Lycanthrope(pawn, LycanthropeUtility.GetLycanthropeHediff(pawn));
        }
    }
}
