using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompUsable_WolfsbloodAltarFeralWerewolfRaid : CompUsable_WolfsbloodAltar
    {
        public override AcceptanceReport CanBeUsedBy(Pawn p, bool forced = false, bool ignoreReserveAndReachable = false)
        {
            Faction faction = WerewolfUtility.GetFeralWerewolfFaction();
            if (faction == null)
            {
                return "Mashed_Bloodmoon_FeralWerewolfFactionMissing".Translate();
            }

            IncidentParms parms = WerewolfUtility.WerewolfRaidParms(parent.Map);
            IncidentWorker_RaidEnemy temp = new IncidentWorker_RaidEnemy();

            if (!temp.FactionCanBeGroupSource(faction, parms))
            {
                return "Mashed_Bloodmoon_FeralWerewolfFactionCantRaid".Translate();
            }

            return base.CanBeUsedBy(p, forced, ignoreReserveAndReachable);
        }
    }
}
