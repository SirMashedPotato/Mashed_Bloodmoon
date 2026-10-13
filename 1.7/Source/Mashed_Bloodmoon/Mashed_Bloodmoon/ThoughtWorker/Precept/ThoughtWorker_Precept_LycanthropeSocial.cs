using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class ThoughtWorker_Precept_LycanthropeSocial : ThoughtWorker_Precept_Social
    {
        protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
        {
            if (!ModsConfig.IdeologyActive)
            {
                return ThoughtState.Inactive;
            }
            return LycanthropeUtility.PawnIsLycanthrope(otherPawn);
        }
    }
}
