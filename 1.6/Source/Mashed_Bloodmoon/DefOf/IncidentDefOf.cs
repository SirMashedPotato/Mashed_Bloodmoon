using RimWorld;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class IncidentDefOf
    {
        static IncidentDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(IncidentDefOf));
        }

        public static IncidentDef Mashed_Bloodmoon_WerewolfAmbush;
    }
}
