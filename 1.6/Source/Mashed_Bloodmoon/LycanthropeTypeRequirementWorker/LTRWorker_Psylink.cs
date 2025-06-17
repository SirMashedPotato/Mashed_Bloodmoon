using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTRWorker_Psylink : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (!ModsConfig.RoyaltyActive)
            {
                return true;
            }
            if (pawn.HasPsylink && pawn.GetPsylinkLevel() >= level)
            {
                return true;
            }
            return "Mashed_Bloodmoon_LTR_MissingPsylink".Translate(level);
        }

        public int level = 1;
    }
}
