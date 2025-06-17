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
            SpectralWerewolfSpawner spectralWerewolfSpawner = (SpectralWerewolfSpawner)ThingMaker.MakeThing(ThingDefOf.Mashed_Bloodmoon_SpectralWerewolfSpawner);
            spectralWerewolfSpawner.werewolfLifespan = (int)(pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropicStressMax) * LycanthropeUtility.lycanthropeStressRate);
            GenSpawn.Spawn(spectralWerewolfSpawner, pawn.Position, pawn.Map);

            pawn.health.RemoveHediff(hediff);
            pawn.jobs.StartJob(new Job(JobDefOf.Vomit), JobCondition.Succeeded);
            Messages.Message("Mashed_Bloodmoon_LycanthropeCured".Translate(pawn), pawn, MessageTypeDefOf.PositiveEvent);
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
