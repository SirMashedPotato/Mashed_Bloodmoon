using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTRWorker_Hediff : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (hediffDef != null && pawn.health.hediffSet.HasHediff(hediffDef))
            {
                return true;
            }

            if (!hediffDefs.NullOrEmpty())
            {
                foreach (HediffDef def in hediffDefs)
                {
                    if (pawn.health.hediffSet.HasHediff(def))
                    {
                        return minSeverity == -1f || pawn.health.hediffSet.GetFirstHediffOfDef(def).Severity >= minSeverity;
                    }
                }
            }
            return "Mashed_Bloodmoon_LTR_InvalidHediff".Translate();
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (hediffDef == null && hediffDefs.NullOrEmpty())
            {
                yield return "both hediffDef and hediffDefs are null";
            }
            if (hediffDef != null && !hediffDefs.NullOrEmpty())
            {
                yield return "use either hediffDef or hediffDefs";
            }
        }

        public HediffDef hediffDef;
        public List<HediffDef> hediffDefs;
        public float minSeverity = -1f;
    }
}
