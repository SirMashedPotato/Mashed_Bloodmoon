﻿using System;
using Verse;

namespace Mashed_Bloodmoon
{
    public class IngestionOutcomeDoer_PotionWolfsblood : IngestionOutcomeDoer_LycanthropePotion
    {
        public override void IngestionOutcome_Dormant(Pawn pawn, Hediff hediff)
        {
            ///TODO force transformation with rage
            throw new NotImplementedException();
        }

        public override void IngestionOutcome_Human(Pawn pawn)
        {
            pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_SaniesLupinus).Severity = 0.1f;
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
