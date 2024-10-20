using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class PawnRenderNodeWorker_LycanthropeTransformed : PawnRenderNodeWorker
    {
        public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
        {
            return true;
        }

        /// <summary>
        /// Hides the pawns head, body, clothes etc
        /// </summary>
        public override void AppendDrawRequests(PawnRenderNode node, PawnDrawParms parms, List<PawnGraphicDrawRequest> requests)
        {
            for(int i = requests.Count - 1; i >= 0; i--)
            {
                if (!(requests[i].node is PawnRenderNode_Lycanthrope || requests[i].node.Worker is PawnRenderNodeWorker_Overlay))
                {
                    requests.RemoveAt(i);
                }
            }
            base.AppendDrawRequests(node, parms, requests);
        }

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

        private HediffComp_Lycanthrope CompLycanthrope(Pawn pawn)
        {
            if (compLycanthrope == null)
            {
                compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            }
            return compLycanthrope;
        }

        private HediffComp_Lycanthrope compLycanthrope;
        private Vector3 scaleFor = Vector3.zero;
    }
}
