using RimWorld;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class TraitDefOf
    {
        static TraitDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(TraitDefOf));
        }
        public static TraitDef Mashed_Bloodmoon_UncontrollableLycanthropy;
    }
}