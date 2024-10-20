using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTRWorker_TransformationCount : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (pawn.records.GetValue(RecordDefOf.Mashed_Bloodmoon_TransformationCount) >= transformationCount)
            {
                return true;
            }
            return "Mashed_Bloodmoon_LTR_InvalidTransformationCount".Translate();
        }

        public int transformationCount = 0;
    }
}
