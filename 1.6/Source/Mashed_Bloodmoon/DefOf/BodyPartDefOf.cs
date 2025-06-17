using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class BodyPartDefOf
    {
        static BodyPartDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartDefOf));
        }
        public static BodyPartDef Jaw;
    }
}