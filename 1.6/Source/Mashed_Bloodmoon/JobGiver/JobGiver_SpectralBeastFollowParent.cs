using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class JobGiver_SpectralBeastFollowParent : JobGiver_AIFollowPawn
    {
        protected override int FollowJobExpireInterval => 200;

        protected override Pawn GetFollowee(Pawn pawn)
        {
            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_SpectralBeast);
            if (hediff != null)
            {
                return hediff.TryGetComp<HediffComp_SummonedBeast>().linkedPawn;
            }
            return null;
        }

        protected override float GetRadius(Pawn pawn)
        {
            return 5f;
        }
    }
}
