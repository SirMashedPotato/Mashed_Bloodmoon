using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    internal class RequirementWorker_BeastHuntCount : LycanthropeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            if (compLycanthrope.completedBeastHunts >= requiredCount)
            {
                return true;
            }
            return "Mashed_Bloodmoon_RequirementWorker_InvalidBeastHuntCount".Translate(requiredCount);
        }

        public int requiredCount;
    }
}
