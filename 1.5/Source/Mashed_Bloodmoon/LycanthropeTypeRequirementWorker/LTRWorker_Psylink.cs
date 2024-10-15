using Verse;

namespace Mashed_Bloodmoon
{
    public class LTRWorker_Psylink : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (ModsConfig.RoyaltyActive && !pawn.HasPsylink)
            {
                return "Mashed_Bloodmoon_LTR_MissingPsylink".Translate();
            }
            return true;
        }
    }
}
