using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace Mashed_Bloodmoon
{
    internal static class WerewolfUtility
    {
        internal static Faction GetFeralWerewolfFaction()
        {
            return FactionUtility.DefaultFactionFrom(FactionDefOf.Mashed_Bloodmoon_FeralWerewolves);
        }

        internal static void GenerateWerewolfPackQuest(out Quest quest)
        {
            Slate slate = new Slate();
            slate.Set("points", StorytellerUtility.DefaultThreatPointsNow(Find.World));
            quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.Mashed_Bloodmoon_OpportunitySite_FeralWerewolfPack, slate);
            QuestUtility.SendLetterQuestAvailable(quest);
        }

        internal static void TriggerWerewolfAmbush()
        {
            IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentDefOf.Mashed_Bloodmoon_WerewolfAmbush.category, Find.WorldObjects.Caravans.RandomElement());
            incidentParms.forced = true;
            incidentParms.faction = GetFeralWerewolfFaction();
            IncidentDef incidentDef = IncidentDefOf.Mashed_Bloodmoon_WerewolfAmbush;
            incidentParms.points *= Mashed_Bloodmoon_ModSettings.HuntsmanMoon_AmbushPointsMultiplier;
            if (incidentParms.points < 200)
            {
                incidentParms.points = 200;
            }
            incidentDef.Worker.TryExecute(incidentParms);
        }

        internal static IncidentParms WerewolfRaidParms(Map map)
        {
            IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(RimWorld.IncidentDefOf.RaidEnemy.category, map);
            incidentParms.forced = true;
            incidentParms.faction = GetFeralWerewolfFaction();
            incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
            incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkInGroups;
            incidentParms.points *= Mashed_Bloodmoon_ModSettings.HuntsmanMoon_RaidPointsMultiplier;
            if (incidentParms.points < 200)
            {
                incidentParms.points = 200;
            }
            return incidentParms;
        }

        internal static void TriggerWerewolfRaid(Map map)
        {
            IncidentParms incidentParms = WerewolfRaidParms(map);
            IncidentDef incidentDef = RimWorld.IncidentDefOf.RaidEnemy;
            incidentDef.Worker.TryExecute(incidentParms);
        }
    }
}
