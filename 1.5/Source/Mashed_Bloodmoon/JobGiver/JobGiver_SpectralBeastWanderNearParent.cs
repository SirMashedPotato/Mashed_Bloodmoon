using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class JobGiver_SpectralBeastWanderNearParent : JobGiver_Wander
    {
        protected override IntVec3 GetWanderRoot(Pawn pawn)
        {
            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_SpectralBeast);
            if (hediff != null)
            {
                return WanderUtility.BestCloseWanderRoot(hediff.TryGetComp<HediffComp_SummonedBeast>().linkedPawn.PositionHeld, pawn);
            }
            return WanderUtility.BestCloseWanderRoot(pawn.PositionHeld, pawn);
        }
    }
}
