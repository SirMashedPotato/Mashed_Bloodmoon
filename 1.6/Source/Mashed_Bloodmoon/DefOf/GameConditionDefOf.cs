using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class GameConditionDefOf
    {
        static GameConditionDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(GameConditionDefOf));
        }
        public static GameConditionDef Mashed_Bloodmoon_HuntsmansMoonPrecursor;
    }
}
