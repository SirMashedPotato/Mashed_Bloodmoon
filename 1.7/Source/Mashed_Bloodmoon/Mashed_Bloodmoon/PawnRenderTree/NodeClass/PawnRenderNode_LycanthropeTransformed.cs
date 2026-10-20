using Verse;

namespace Mashed_Bloodmoon
{
    public class PawnRenderNode_LycanthropeTransformed : PawnRenderNode_Lycanthrope
    {
        public PawnRenderNode_LycanthropeTransformed(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
        {
        }

        public override Graphic GraphicFor(Pawn pawn)
        {
            LycanthropeBeastFormDef typeDef = CompLycanthrope(pawn).BeastFormDef;
            return GraphicDatabase.Get<Graphic_Multi>(typeDef.graphicData.texPath, ShaderDatabase.CutoutComplex, typeDef.graphicData.drawSize,
                CompLycanthrope(pawn).primaryColour, CompLycanthrope(pawn).secondaryColour);
        }
    }
}
