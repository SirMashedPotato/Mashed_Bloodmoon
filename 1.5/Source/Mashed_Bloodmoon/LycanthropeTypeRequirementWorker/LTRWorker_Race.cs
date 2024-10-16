using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTRWorker_Race : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (raceDef != null && pawn.def == raceDef)
            {
                return true;
            }

            if (!raceDefs.NullOrEmpty() && raceDefs.Contains(pawn.def))
            {
                return true;
            }
            return "Mashed_Bloodmoon_LTR_InvalidRace".Translate();
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (raceDef == null && raceDefs.NullOrEmpty())
            {
                yield return "both raceDef and raceDefs are null";
            }
            if (raceDef != null && !raceDefs.NullOrEmpty())
            {
                yield return "use either raceDef or raceDefs";
            }
        }

        public ThingDef raceDef;
        public List<ThingDef> raceDefs;
    }
}
