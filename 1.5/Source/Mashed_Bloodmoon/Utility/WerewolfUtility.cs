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
    }
}
