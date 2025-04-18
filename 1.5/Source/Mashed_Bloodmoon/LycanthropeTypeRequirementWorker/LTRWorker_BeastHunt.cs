﻿using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    internal class LTRWorker_BeastHunt : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            if (beastHuntDef.Completed(compLycanthrope))
            {
                return true;
            }
            return "Mashed_Bloodmoon_LTR_InvalidBeastHunt".Translate(beastHuntDef.LabelCap);
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (beastHuntDef == null)
            {
                yield return "beastHuntDef is null";
            }
        }

        public LycanthropeBeastHuntDef beastHuntDef;
    }
}
