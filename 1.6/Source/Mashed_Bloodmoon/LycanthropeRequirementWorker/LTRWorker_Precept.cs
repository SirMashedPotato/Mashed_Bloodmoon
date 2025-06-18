using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTRWorker_Precept : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (ModsConfig.IdeologyActive && pawn.Ideo != null)
            {
                if (preceptDef != null && pawn.Ideo.HasPrecept(preceptDef))
                {
                    return true;
                }
                if (!preceptDefs.NullOrEmpty())
                {
                    foreach (PreceptDef def in preceptDefs)
                    {
                        if (pawn.Ideo.HasPrecept(def))
                        {
                            return true;
                        }
                    }
                }
                return "Mashed_Bloodmoon_LTR_MissingIdeoOneOf".Translate() + DoMissingList(preceptDefs);
            }
            return "Mashed_Bloodmoon_LTR_MissingIdeo".Translate(preceptDef);
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (preceptDef == null && preceptDefs.NullOrEmpty())
            {
                yield return "both preceptDef and preceptDefs are null";
            }
            if (preceptDef != null && !preceptDefs.NullOrEmpty())
            {
                yield return "use either preceptDef or preceptDefs";
            }
        }

        public PreceptDef preceptDef;
        public List<PreceptDef> preceptDefs;
    }
}
