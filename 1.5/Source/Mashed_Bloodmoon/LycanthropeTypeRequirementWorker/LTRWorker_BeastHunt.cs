using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    internal class LTRWorker_BeastHunt : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            if (compLycanthrope.beastHuntTracker.TryGetValue(beastHuntDef, out int currentUsedCount) && currentUsedCount >= beastHuntDef.consumeCount)
            {
                return true;
            }
            return "Mashed_Bloodmoon_LTR_InvalidBeastHunt".Translate(beastHuntDef);
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
