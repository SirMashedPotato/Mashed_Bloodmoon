using RimWorld;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class FactionDefOf
    {
        static FactionDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(FactionDefOf));
        }
        public static FactionDef Mashed_Bloodmoon_FeralWerewolves;
    }
}
