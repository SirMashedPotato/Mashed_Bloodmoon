using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class PawnRenderNode_LycanthropeTransformedGlow : PawnRenderNode_Lycanthrope
    {
        public PawnRenderNode_LycanthropeTransformedGlow(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
        {
        }

        public override Graphic GraphicFor(Pawn pawn)
        {
            LycanthropeTypeDef typeDef = CompLycanthrope(pawn).LycanthropeTypeDef;
            return GraphicDatabase.Get<Graphic_Multi>(typeDef.graphicData.texPath + "Glow", ShaderDatabase.TransparentPostLight, typeDef.graphicData.drawSize, Color.white);
        }
    }
}
