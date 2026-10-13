using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class ThoughtWorker_PreceptLycanthropePresent : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (!ModsConfig.IdeologyActive || LycanthropeUtility.PawnIsLycanthrope(p))
            {
                return ThoughtState.Inactive;
            }
            foreach (Pawn pawn in p.MapHeld.mapPawns.AllPawnsSpawned)
            {
                if (LycanthropeUtility.PawnIsLycanthrope(pawn) && (pawn.IsPrisonerOfColony || pawn.IsSlaveOfColony || pawn.IsColonist))
                {
                    return ThoughtState.ActiveDefault;
                }
            }
            return ThoughtState.Inactive;
        }
    }
}
