using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    internal class JobGiver_NightTransformation : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            compLycanthrope.TransformPawn(true);
            return JobMaker.MakeJob(RimWorld.JobDefOf.Wait, 10);
        }
    }
}
