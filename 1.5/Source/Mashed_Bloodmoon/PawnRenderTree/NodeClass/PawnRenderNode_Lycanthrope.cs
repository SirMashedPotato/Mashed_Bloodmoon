using Verse;

namespace Mashed_Bloodmoon
{
    public abstract class PawnRenderNode_Lycanthrope : PawnRenderNode
    {
        protected PawnRenderNode_Lycanthrope(Pawn pawn, PawnRenderNodeProperties props, Verse.PawnRenderTree tree) : base(pawn, props, tree)
        {
        }

        public HediffComp_Lycanthrope CompLycanthrope(Pawn pawn)
        {
            if (compLycanthrope == null)
            {
                compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            }
            return compLycanthrope;
        }

        private HediffComp_Lycanthrope compLycanthrope;
    }
}
