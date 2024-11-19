using RimWorld;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class LycanthropeTotemDefOf
    {
        static LycanthropeTotemDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(LycanthropeTotemDefOf));
        }
        public static LycanthropeTotemDef Mashed_Bloodmoon_ConsumedHearts;
    }
}