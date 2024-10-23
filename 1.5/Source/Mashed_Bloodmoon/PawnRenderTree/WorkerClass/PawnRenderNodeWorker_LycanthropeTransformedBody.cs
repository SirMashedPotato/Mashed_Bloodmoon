using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class PawnRenderNodeWorker_LycanthropeTransformedBody : PawnRenderNodeWorker_LycanthropeScaled
    {
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
    }
}
