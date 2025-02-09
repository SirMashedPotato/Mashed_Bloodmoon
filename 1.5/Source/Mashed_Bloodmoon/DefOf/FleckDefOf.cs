using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class FleckDefOf
    {
        static FleckDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(FleckDefOf));
        }
        public static FleckDef Mashed_Bloodmoon_AdrenalineRushFleck;
    }
}