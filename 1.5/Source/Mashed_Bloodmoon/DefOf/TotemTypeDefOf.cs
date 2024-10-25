using RimWorld;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class TotemTypeDefOf
    {
        static TotemTypeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(TotemTypeDefOf));
        }
        public static TotemTypeDef Mashed_Bloodmoon_ConsumedHearts;
    }
}