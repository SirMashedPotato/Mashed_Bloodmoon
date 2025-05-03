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
        public static StatDef Mashed_Bloodmoon_LycanthropeSilverWeakness;
        public static StatDef Mashed_Bloodmoon_LycanthropeWolfsbaneWeakness;
        public static StatDef Mashed_Bloodmoon_LycanthropeKillStressReductionChance;
        public static StatDef Mashed_Bloodmoon_LycanthropeHeartSatiationFactor;
        public static StatDef Mashed_Bloodmoon_SilverDamageFactor;
    }
}