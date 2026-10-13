using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class DesignationDefOf
    {
        static DesignationDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DesignationDefOf));
        }
        public static DesignationDef Mashed_Bloodmoon_ConsumeHeart;
    }
}