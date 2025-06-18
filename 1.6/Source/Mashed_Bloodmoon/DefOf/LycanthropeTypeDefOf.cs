using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class LycanthropeTypeDefOf
    {
        static LycanthropeTypeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(LycanthropeTypeDefOf));
        }
        public static LycanthropeBeastFormDef Mashed_Bloodmoon_Werewolf;
    }
}
