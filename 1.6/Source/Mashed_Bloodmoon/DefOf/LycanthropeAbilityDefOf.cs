using RimWorld;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class LycanthropeAbilityDefOf
    {
        static LycanthropeAbilityDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(LycanthropeAbilityDefOf));
        }
        public static LycanthropeAbilityDef Mashed_Bloodmoon_ConsumeHeart;
    }
}