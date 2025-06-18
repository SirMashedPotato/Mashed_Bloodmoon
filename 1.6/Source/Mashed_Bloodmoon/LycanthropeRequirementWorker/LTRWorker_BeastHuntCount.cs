using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    internal class LTRWorker_BeastHuntCount : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            if (compLycanthrope.completedBeastHunts >= requiredCount)
            {
                return true;
            }
            return "Mashed_Bloodmoon_LTR_InvalidBeastHuntCount".Translate(requiredCount);
        }

        public int requiredCount;
    }
}
