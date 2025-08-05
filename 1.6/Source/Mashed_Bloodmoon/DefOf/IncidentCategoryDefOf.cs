using RimWorld;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class IncidentCategoryDefOf
    {
        static IncidentCategoryDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(IncidentCategoryDefOf));
        }

        public static IncidentCategoryDef Mashed_Bloodmoon_HuntsmansMoon;
    }
}
