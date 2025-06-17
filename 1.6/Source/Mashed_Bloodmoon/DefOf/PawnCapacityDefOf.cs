using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class PawnCapacityDefOf
    {
        static PawnCapacityDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PawnCapacityDefOf));
        }
        public static PawnCapacityDef Eating;
    }
}