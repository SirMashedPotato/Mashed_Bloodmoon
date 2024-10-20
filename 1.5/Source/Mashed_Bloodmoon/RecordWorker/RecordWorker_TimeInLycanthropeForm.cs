using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class RecordWorker_TimeInLycanthropeForm : RecordWorker
    {
        public override bool ShouldMeasureTimeNow(Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformed) != null;
        }
    }
}
