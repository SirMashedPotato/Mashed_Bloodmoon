using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class MentalStateDefOf
    {
        static MentalStateDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(MentalStateDefOf));
        }
        public static MentalStateDef Mashed_Bloodmoon_LycanthropeFury;
        public static MentalStateDef Mashed_Bloodmoon_SpectralBeast;
    }
}