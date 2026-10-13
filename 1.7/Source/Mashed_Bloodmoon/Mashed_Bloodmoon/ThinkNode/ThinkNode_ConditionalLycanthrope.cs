using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class ThinkNode_ConditionalLycanthrope : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            if (!LycanthropeUtility.PawnIsLycanthrope(pawn))
            {
                return false;
            }
            if (onlyIfCanTransform && !TransformationUtility.PawnCanTransform(pawn))
            {
                return false;
            }
            if (onlyIfTransformed && !LycanthropeUtility.PawnIsTransformedLycanthrope(pawn))
            {
                return false;
            }
            if (onlyIfNight && !LycanthropeUtility.IsNight(pawn))
            {
                return false;
            }
            return  true;
        }

        public bool onlyIfCanTransform = false;
        public bool onlyIfTransformed = false;
        public bool onlyIfNight = false;
    }
}
