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
                compLycanthrope?.TransformPawn(true);
            }
            base.PostStart(reason);
        }
    }
}
