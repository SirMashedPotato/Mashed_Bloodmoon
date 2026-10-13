using RimWorld;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class QuestScriptDefOf
    {
        static QuestScriptDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(QuestScriptDefOf));
        }

        public static QuestScriptDef Mashed_Bloodmoon_OpportunitySite_FeralWerewolfPack;
    }
}
