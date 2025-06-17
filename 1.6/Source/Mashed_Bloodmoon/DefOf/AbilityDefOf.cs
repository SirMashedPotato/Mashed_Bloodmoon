using RimWorld;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class AbilityDefOf
    {
        static AbilityDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AbilityDefOf));
        }
        public static AbilityDef Mashed_Bloodmoon_ConsumeHeart;
    }
}