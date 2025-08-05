using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    internal static class HuntsmansMoonUtility
    {
        public static bool GetValidIncidentDef(out IncidentDef incidentDef, IncidentParms parms)
        {
            List<IncidentDef> potentialIncidentList = DefDatabase<IncidentDef>.AllDefsListForReading.Where(
                x => x.category == IncidentCategoryDefOf.Mashed_Bloodmoon_HuntsmansMoon && x.Worker.CanFireNow(parms)).ToList();
            if (potentialIncidentList.NullOrEmpty())
            {
                incidentDef = null;
                return false;
            }

            incidentDef = potentialIncidentList.RandomElementByWeight(x => x.baseChance);
            return true;
        }
    }
}
