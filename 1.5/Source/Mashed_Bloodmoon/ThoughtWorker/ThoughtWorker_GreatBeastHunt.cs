using RimWorld;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    public class ThoughtWorker_GreatBeastHunt : ThoughtWorker
    {

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (!LycanthropeUtility.PawnIsLycanthrope(p))
            {
                return ThoughtState.Inactive;
            }

            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(p);

            if (compLycanthrope.greatBeastHeartTracker.NullOrEmpty())
            {
                return ThoughtState.Inactive;
            }

            int count = CompletedCount(p);
            if (count > 0)
            {
                return ThoughtState.ActiveAtStage(0, count.ToString());
            }
            return ThoughtState.Inactive;
        }

        public override float MoodMultiplier(Pawn p)
        {
            return CompletedCount(p);
        }

        public int CompletedCount(Pawn p)
        {
            return LycanthropeUtility.GetCompLycanthrope(p).greatBeastHeartTracker.Where(x => x.Key.Completed(x.Value)).Count();
        }
    }
}
