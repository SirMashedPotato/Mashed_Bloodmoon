using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTRWorker_Race : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (!raceDefs.NullOrEmpty() && !raceDefs.Contains(pawn.def))
            {
                return "Mashed_Bloodmoon_LTR_InvalidRace".Translate();
            }
            return true;
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (raceDefs.NullOrEmpty())
            {
                yield return "null raceDefs";
            }
        }

        public List<ThingDef> raceDefs;
    }
}
