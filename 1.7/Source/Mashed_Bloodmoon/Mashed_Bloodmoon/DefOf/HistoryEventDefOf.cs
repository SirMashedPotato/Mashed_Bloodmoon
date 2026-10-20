using RimWorld;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class HistoryEventDefOf
    {
        static HistoryEventDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(HistoryEventDefOf));
        }

        [MayRequireIdeology]
        public static HistoryEventDef Mashed_Bloodmoon_LycanthropeDied;
    }
}
