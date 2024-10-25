using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class ThoughtWorker_PreceptIsLycanthrope : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (!ModsConfig.IdeologyActive)
            {
                return ThoughtState.Inactive;
            }
            return LycanthropeUtility.PawnIsLycanthrope(p);
        }
    }
}
