using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class ThingDefOf
    {
        static ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
        }
        public static ThingDef Mashed_Bloodmoon_TransformEffect;
        public static ThingDef Mashed_Bloodmoon_SpectralWerewolfSpawner;
    }
}