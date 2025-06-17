using RimWorld;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class RecordDefOf
    {
        static RecordDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RecordDefOf));
        }
        public static RecordDef Mashed_Bloodmoon_TransformationCount;
        public static RecordDef Mashed_Bloodmoon_TransformationTime;
        public static RecordDef Mashed_Bloodmoon_HeartsConsumed;
        public static RecordDef Mashed_Bloodmoon_BeastHuntsCompleted;
        public static RecordDef Mashed_Bloodmoon_PawnsKilledTransformed;
    }
}
