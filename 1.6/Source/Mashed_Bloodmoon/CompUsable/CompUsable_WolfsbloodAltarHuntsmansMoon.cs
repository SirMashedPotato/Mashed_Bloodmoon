using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompUsable_WolfsbloodAltarHuntsmansMoon : CompUsable_WolfsbloodAltar
    {
        public override AcceptanceReport CanBeUsedBy(Pawn p, bool forced = false, bool ignoreReserveAndReachable = false)
        {
            if (p.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Mashed_Bloodmoon_HuntsmansMoonPrecursor))
            {
                return "Mashed_Bloodmoon_HuntsmansMoonPrecursorActive".Translate();
            }

            IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Mashed_Bloodmoon_HuntsmansMoon, Find.World);
            if (!IncidentDefOf.Mashed_Bloodmoon_HuntsmansMoon.Worker.CanFireNow(parms))
            {
                return "Mashed_Bloodmoon_HuntsmansMoonCantTrigger".Translate();
            }

            return base.CanBeUsedBy(p, forced, ignoreReserveAndReachable);
        }
    }
}
