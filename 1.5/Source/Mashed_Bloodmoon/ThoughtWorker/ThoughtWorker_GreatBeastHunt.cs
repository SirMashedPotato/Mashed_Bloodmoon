using RimWorld;
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

            return ThoughtState.ActiveDefault;
        }

        public override float MoodMultiplier(Pawn p)
        {
            return LycanthropeUtility.GetCompLycanthrope(p).greatBeastHeartTracker.Count;
        }
    }
}
