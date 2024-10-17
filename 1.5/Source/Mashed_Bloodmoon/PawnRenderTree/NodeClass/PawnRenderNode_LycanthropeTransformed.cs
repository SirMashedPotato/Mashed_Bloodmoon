﻿using Verse;

namespace Mashed_Bloodmoon
{
    public class PawnRenderNode_LycanthropeTransformed : PawnRenderNode
    {
        public PawnRenderNode_LycanthropeTransformed(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
        {
        }

        public override Graphic GraphicFor(Pawn pawn)
        {
            LycanthropeTypeDef typeDef = CompLycanthrope(pawn).lycanthropeTypeDef;
            return GraphicDatabase.Get<Graphic_Multi>(typeDef.graphicData.texPath, ShaderDatabase.CutoutComplex, typeDef.graphicData.drawSize, 
                typeDef.graphicData.color, typeDef.graphicData.colorTwo);
        }

        private HediffComp_Lycanthrope CompLycanthrope(Pawn pawn)
        {
            if (compLycanthrope == null)
            {
                compLycanthrope = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_Lycanthrope).TryGetComp<HediffComp_Lycanthrope>();
            }
            return compLycanthrope;
        }

        private HediffComp_Lycanthrope compLycanthrope;
    }
}
