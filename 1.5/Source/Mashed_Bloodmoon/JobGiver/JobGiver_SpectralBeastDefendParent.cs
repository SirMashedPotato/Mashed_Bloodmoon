using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class JobGiver_SpectralBeastDefendParent : JobGiver_AIDefendPawn
    {
        protected override Pawn GetDefendee(Pawn pawn)
        {
            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_SpectralBeast);
            if (hediff != null)
            {
                return hediff.TryGetComp<HediffComp_SummonedBeast>().parentPawn;
            }
            return null;
        }

        protected override float GetFlagRadius(Pawn pawn)
        {
            return 5f;
        }
    }
}
