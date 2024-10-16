using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTRWorker_Hediff : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (!hediffDefs.NullOrEmpty())
            {
                foreach (HediffDef hediffDef in hediffDefs)
                {
                    if (pawn.health.hediffSet.HasHediff(hediffDef))
                    {
                        return true;
                    }
                }
                return "Mashed_Bloodmoon_LTR_InvalidHediff".Translate();
            }
            return true;
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (hediffDefs.NullOrEmpty())
            {
                yield return "null hediffDefs";
            }
        }

        public List<HediffDef> hediffDefs;
    }
}
