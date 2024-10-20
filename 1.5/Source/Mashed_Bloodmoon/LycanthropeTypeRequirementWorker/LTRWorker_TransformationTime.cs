using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTRWorker_TransformationTime : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if ((pawn.records.GetValue(RecordDefOf.Mashed_Bloodmoon_TransformationTime) / GenDate.TicksPerHour) >= hoursTransformed)
            {
                return true;
            }
            return "Mashed_Bloodmoon_LTR_InvalidTransformationTime".Translate();
        }

        public int hoursTransformed = 0;
    }
}
