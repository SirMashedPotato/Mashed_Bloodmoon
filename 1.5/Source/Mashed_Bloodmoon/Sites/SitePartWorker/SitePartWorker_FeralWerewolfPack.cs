using Mashed_Bloodmoon.Utility;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace Mashed_Bloodmoon
{
    public class SitePartWorker_FeralWerewolfPack : SitePartWorker
    {
        public override bool IsAvailable()
        {
            if (base.IsAvailable())
            {
                return WerewolfUtility.GetFeralWerewolfFaction() != null;
            }
            return false;
        }

        public override void Init(Site site, SitePart sitePart)
        {
            base.Init(site, sitePart);
            sitePart.expectedEnemyCount = GetWerewolfCount(site, sitePart.parms);
        }

        public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
        {
            string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out _);
            lookTargets = new LookTargets(map.Parent);
            return arrivedLetterPart;
        }

        public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
        {
            base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
            int werewolfCount = GetWerewolfCount(part.site, part.parms);
            outExtraDescriptionRules.Add(new Rule_String("count", werewolfCount.ToString()));
            outExtraDescriptionConstants.Add("count", werewolfCount.ToString());
        }

        public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
        {
            return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + "KnownSiteThreatEnemyCountAppend".Translate(GetWerewolfCount(site, sitePart.parms), "Enemies".Translate());
        }

        public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
        {
            SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
            sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints * 2f, FactionDefOf.Mashed_Bloodmoon_FeralWerewolves.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat));
            return sitePartParams;
        }

        private int GetWerewolfCount(Site site, SitePartParams parms)
        {
            return PawnGroupMakerUtility.GeneratePawnKindsExample(new PawnGroupMakerParms
            {
                tile = site.Tile,
                faction = WerewolfUtility.GetFeralWerewolfFaction(),
                groupKind = PawnGroupKindDefOf.Combat,
                points = parms.threatPoints,
                seed = parms.randomValue
            }).Count();
        }
    }
}
