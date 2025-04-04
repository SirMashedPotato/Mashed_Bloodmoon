﻿using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    internal class LTRWorker_Totem : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            if (compLycanthrope.usedTotemTracker.TryGetValue(totemTypeDef, out int currentUsedCount) && currentUsedCount >= usedCount)
            {
                return true;
            }
            return "Mashed_Bloodmoon_LTR_InvalidTotemCount".Translate(totemTypeDef, usedCount);
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (totemTypeDef == null)
            {
                yield return "totemTypeDef is null";
            }
        }

        public LycanthropeTotemDef totemTypeDef;
        public int usedCount = 1;
    }
}
