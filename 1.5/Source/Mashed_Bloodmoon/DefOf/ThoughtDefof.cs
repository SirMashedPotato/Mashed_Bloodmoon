using RimWorld;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class ThoughtDefOf
    {
        static ThoughtDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThoughtDefOf));
        }
        public static ThoughtDef Mashed_Bloodmoon_RestlessSleep;
    }
}