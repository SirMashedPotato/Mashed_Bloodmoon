using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class PawnRenderNodeWorker_LycanthropeScaled : PawnRenderNodeWorker
    {
        /// <summary>
        /// Scales the werewolf texture, because otherwise it's pawn sized
        /// </summary>
        public override Vector3 ScaleFor(PawnRenderNode node, PawnDrawParms parms)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(parms.pawn);
            Vector2 drawSize = compLycanthrope.BeastFormDef.graphicData.drawSize;


            switch (Mashed_Bloodmoon_ModSettings.Lycanthropy_TextureScalingMode)
            {
                case BeastFormScalingMode.BodySize:
                    return BodySizeScaling(parms, compLycanthrope, drawSize);

                case BeastFormScalingMode.DrawSize:
                    return DrawSizeScaling(parms, compLycanthrope, drawSize);

                case BeastFormScalingMode.BigAndSmall:
                    if (parms.pawn.genes != null && parms.pawn.genes.GenesListForReading.Any(x=> !x.def.exclusionTags.NullOrEmpty() && x.def.exclusionTags.Contains("BodySize")))
                    {
                        return BodySizeScaling(parms, compLycanthrope, drawSize);
                    }
                    return DrawSizeScaling(parms, compLycanthrope, drawSize);

                default:
                    return drawSize;
            }
        }

        private Vector3 BodySizeScaling(PawnDrawParms parms, HediffComp_Lycanthrope compLycanthrope, Vector2 drawSize)
        {
            return new Vector3(drawSize.x * parms.pawn.BodySize, 0, drawSize.y * parms.pawn.BodySize);
        }

        private Vector3 DrawSizeScaling(PawnDrawParms parms, HediffComp_Lycanthrope compLycanthrope, Vector2 drawSize)
        {
            return new Vector3(drawSize.x * parms.pawn.DrawSize.x, 0, drawSize.y * parms.pawn.DrawSize.y);
        }
    }
}
