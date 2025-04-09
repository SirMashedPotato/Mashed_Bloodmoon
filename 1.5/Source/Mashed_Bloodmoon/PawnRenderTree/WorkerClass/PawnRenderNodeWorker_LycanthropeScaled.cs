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
            Vector2 drawSize = compLycanthrope.LycanthropeTypeDef.graphicData.drawSize;
            return new Vector3(drawSize.x * parms.pawn.DrawSize.x, 0, drawSize.y * parms.pawn.DrawSize.y);
        }
    }
}
