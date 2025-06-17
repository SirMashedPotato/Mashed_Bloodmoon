using Verse;

namespace Mashed_Bloodmoon
{
    public class CompUsable_WolfsbloodAltarFeralWerewolfRaid : CompUsable_WolfsbloodAltar
    {
        public override AcceptanceReport CanBeUsedBy(Pawn p, bool forced = false, bool ignoreReserveAndReachable = false)
        {
            if (WerewolfUtility.GetFeralWerewolfFaction() == null)
            {
                return "Mashed_Bloodmoon_FeralWerewolfFactionMissing".Translate();
            }

            return base.CanBeUsedBy(p, forced, ignoreReserveAndReachable);
        }
    }
}
