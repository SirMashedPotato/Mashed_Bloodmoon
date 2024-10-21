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
            HediffComp_Lycanthrope comp = CompLycanthrope(pawn);
            LycanthropeTypeDef typeDef = comp.LycanthropeTypeDef;
            return GraphicDatabase.Get<Graphic_Multi>(typeDef.graphicData.texPath, ShaderDatabase.CutoutComplex, typeDef.graphicData.drawSize,
                comp.primaryColour, comp.secondaryColour);
        }
    }
}
