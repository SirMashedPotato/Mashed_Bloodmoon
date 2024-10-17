using Verse;

namespace Mashed_Bloodmoon
{
    public static class LycanthropeUtility
    {
        public static HediffComp_Lycanthrope GetCompLycanthrope(Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_Lycanthrope).TryGetComp<HediffComp_Lycanthrope>();
        }
    }
}
