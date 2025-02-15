using RimWorld;

namespace Mashed_Bloodmoon.Utility
{
    internal static class WerewolfUtility
    {
        internal static Faction GetFeralWerewolfFaction()
        {
            return FactionUtility.DefaultFactionFrom(FactionDefOf.Mashed_Bloodmoon_FeralWerewolves);
        }
    }
}
