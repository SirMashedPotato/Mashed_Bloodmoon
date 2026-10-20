using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class PawnRenderNode_LycanthropePrimaryColor : PawnRenderNode_Lycanthrope
    {
        public PawnRenderNode_LycanthropePrimaryColor(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
        {
        }

        public override Color ColorFor(Pawn pawn)
        {
            return CompLycanthrope(pawn).primaryColour;
        }
    }
}
