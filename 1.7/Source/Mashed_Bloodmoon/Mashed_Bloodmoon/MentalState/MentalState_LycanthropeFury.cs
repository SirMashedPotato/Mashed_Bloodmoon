using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class MentalState_LycanthropeFury : MentalState_Berserk
    {
        public override void PostStart(string reason)
        {
            if (!LycanthropeUtility.PawnIsTransformedLycanthrope(pawn))
            {
                HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
                if (compLycanthrope == null)
                {
                    pawn.ClearMind_NewTemp();
                    return;
                }
                compLycanthrope?.TransformPawn(true);
            }
            base.PostStart(reason);
        }

        public override void PostEnd()
        {
            Hediff transformedHediff = LycanthropeUtility.GetTransformedHediff(pawn);
            if (transformedHediff != null)
            {
                pawn.health.RemoveHediff(transformedHediff);
            }
            base.PostEnd();
        }
    }
}
