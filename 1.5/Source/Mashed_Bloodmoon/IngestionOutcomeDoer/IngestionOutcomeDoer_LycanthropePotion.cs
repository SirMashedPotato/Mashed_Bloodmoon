using Verse;
using RimWorld;

namespace Mashed_Bloodmoon
{
    public abstract class IngestionOutcomeDoer_LycanthropePotion : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_SaniesLupinus);
            if (hediff != null)
            {
                IngestionOutcome_SaniesLupinus(pawn, hediff);
                return;
            }

            hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeDormant);
            if (hediff != null)
            {
                IngestionOutcome_Dormant(pawn, hediff);
                return;
            }

            hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformed);
            if (hediff != null)
            {
                IngestionOutcome_Transformed(pawn, hediff);
                return;
            }

            hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_Lycanthrope);
            if (hediff != null)
            {
                IngestionOutcome_Lycanthrope(pawn, hediff);
                return;
            }

            IngestionOutcome_Human(pawn);
        }

        /// <summary>
        /// When a pawn with dormant lycanthropy ingests it
        /// </summary>
        public abstract void IngestionOutcome_Dormant(Pawn pawn, Hediff hediff);

        /// <summary>
        /// When a pawn without sanies lupinus, or any form of lycanthropy, ingests it
        /// </summary>
        public abstract void IngestionOutcome_Human(Pawn pawn);

        /// <summary>
        /// When a lycanthrope ingests it
        /// </summary>
        public abstract void IngestionOutcome_Lycanthrope(Pawn pawn, Hediff hediff);

        /// <summary>
        /// When a pawn with sanies lupinus ingests it
        /// </summary>
        public abstract void IngestionOutcome_SaniesLupinus(Pawn pawn, Hediff hediff);

        /// <summary>
        /// When a transformed pawn ingests it
        /// </summary>
        public abstract void IngestionOutcome_Transformed(Pawn pawn, Hediff hediff);
    }
}
