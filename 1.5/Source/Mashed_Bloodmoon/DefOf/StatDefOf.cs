using RimWorld;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class StatDefOf
    {
        static StatDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(StatDefOf));
        }
        public static StatDef Mashed_Bloodmoon_LycanthropyResistance;
        public static StatDef Mashed_Bloodmoon_LycanthropicStressMax;
    }
}