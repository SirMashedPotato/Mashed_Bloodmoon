﻿using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class IngestionOutcomeDoer_PotionWolfsblood : IngestionOutcomeDoer_LycanthropePotion
    {
        public override void IngestionOutcome_Dormant(Pawn pawn, Hediff hediff)
        {
            LycanthropeUtility.ForceTransformation(pawn, hediff);
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
            pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_WolfsbloodAdrenaline).Severity = 1f;
        }

        public override void IngestionOutcome_SaniesLupinus(Pawn pawn, Hediff hediff)
        {
            hediff.Severity += 0.3f;
        }
    }
}
