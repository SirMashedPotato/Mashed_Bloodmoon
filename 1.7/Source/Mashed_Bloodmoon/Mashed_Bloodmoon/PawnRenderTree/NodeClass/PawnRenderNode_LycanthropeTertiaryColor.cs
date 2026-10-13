using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class PawnRenderNode_LycanthropeTertiaryColor : PawnRenderNode_Lycanthrope
    {
        public PawnRenderNode_LycanthropeTertiaryColor(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
        {
        }

        public override Color ColorFor(Pawn pawn)
        {
            return CompLycanthrope(pawn).tertiaryColour;
        }
    }
}
