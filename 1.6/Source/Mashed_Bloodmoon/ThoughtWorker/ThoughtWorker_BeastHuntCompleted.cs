using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class ThoughtWorker_BeastHuntCompleted : ThoughtWorker
    {

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            int completedCount = CompletedCount(p);
            if (completedCount > 0)
            {
                return ThoughtState.ActiveAtStage(0, completedCount.ToString());
            }
            return ThoughtState.Inactive;
        }

        public override float MoodMultiplier(Pawn p)
        {
            return CompletedCount(p);
        }

        public int CompletedCount(Pawn p)
        {
            return LycanthropeUtility.GetCompLycanthrope(p).completedBeastHuntsCount;
        }
    }
}
