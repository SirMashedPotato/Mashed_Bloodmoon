using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    internal class RequirementWorker_Record : LycanthropeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            float minCount = recordCount;
            if (inHours)
            {
                recordCount *= GenDate.TicksPerHour;
            }
            if (pawn.records.GetValue(recordDef) >= minCount)
            {
                return true;
            }
            return "Mashed_Bloodmoon_RequirementWorker_InvalidRecord".Translate(recordDef.label, minCount);
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (recordDef == null)
            {
                yield return "recordDef is null";
            }
        }

        public RecordDef recordDef;
        public float recordCount = 1f;
        public bool inHours = false;
    }
}
