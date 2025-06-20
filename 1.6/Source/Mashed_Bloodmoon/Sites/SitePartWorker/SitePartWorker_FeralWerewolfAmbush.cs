using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Mashed_Bloodmoon
{
    public class SitePartWorker_FeralWerewolfAmbush : SitePartWorker
    {
        private const float ThreatPointsFactor = 0.8f;

        public override bool IsAvailable()
        {
            if (base.IsAvailable())
            {
                return WerewolfUtility.GetFeralWerewolfFaction() != null;
            }
            return false;
        }

        public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
        {
            SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, WerewolfUtility.GetFeralWerewolfFaction());
            sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints * ThreatPointsFactor, FactionDefOf.Mashed_Bloodmoon_FeralWerewolves.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat));
            return sitePartParams;
        }
    }
}
