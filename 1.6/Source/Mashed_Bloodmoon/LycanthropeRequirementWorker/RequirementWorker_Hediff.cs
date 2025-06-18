using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class RequirementWorker_Hediff : LycanthropeRequirementWorker
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
                return "Mashed_Bloodmoon_RequirementWorker_MissingOneOf".Translate() + DoMissingList(hediffDefs) + " " + "Mashed_Bloodmoon_RequirementWorker_HediffSeverity".Translate(minSeverity.ToStringPercent());
            }
            return "Mashed_Bloodmoon_RequirementWorker_Missing".Translate(hediffDef) + " " + "Mashed_Bloodmoon_RequirementWorker_HediffSeverity".Translate(minSeverity.ToStringPercent());
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
        public float minSeverity = 0f;
    }
}
