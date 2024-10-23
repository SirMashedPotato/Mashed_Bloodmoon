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
            if (scaleFor == Vector3.zero)
            {
                Vector2 drawSize = CompLycanthrope(parms.pawn).LycanthropeTypeDef.graphicData.drawSize;
                scaleFor = new Vector3(drawSize.x * parms.pawn.DrawSize.x, 0, drawSize.y * parms.pawn.DrawSize.y);
            }
            return scaleFor;
        }

        public HediffComp_Lycanthrope CompLycanthrope(Pawn pawn)
        {
            if (compLycanthrope == null)
            {
                compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            }
            return compLycanthrope;
        }

        private Vector3 scaleFor = Vector3.zero;
        private HediffComp_Lycanthrope compLycanthrope;
    }
}
