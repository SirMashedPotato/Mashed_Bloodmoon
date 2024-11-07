using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class PawnRenderNode_LycanthropeSecondaryColor : PawnRenderNode_Lycanthrope
    {
        public PawnRenderNode_LycanthropeSecondaryColor(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
        {
        }

        public override Color ColorFor(Pawn pawn)
        {
            return CompLycanthrope(pawn).secondaryColour;
        }
    }
}
