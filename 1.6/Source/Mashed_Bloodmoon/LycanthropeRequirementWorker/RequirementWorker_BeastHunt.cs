using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    internal class RequirementWorker_BeastHunt : LycanthropeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            if (beastHuntDef.Completed(compLycanthrope))
            {
                return true;
            }
            return "Mashed_Bloodmoon_RequirementWorker_InvalidBeastHunt".Translate(beastHuntDef.LabelCap);
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
