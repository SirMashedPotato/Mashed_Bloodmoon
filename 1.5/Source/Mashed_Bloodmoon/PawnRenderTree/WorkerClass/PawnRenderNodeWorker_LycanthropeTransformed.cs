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

        public override void AppendDrawRequests(PawnRenderNode node, PawnDrawParms parms, List<PawnGraphicDrawRequest> requests)
        {
            base.AppendDrawRequests(node, parms, requests);
        }

        /// <summary>
        /// Scales the werewolf texture, because otherwise it's pawn sized
        /// </summary>
        public override Vector3 ScaleFor(PawnRenderNode node, PawnDrawParms parms)
        {
            if (scaleFor == Vector3.zero)
            {
                Vector2 drawSize = CompLycanthrope(parms.pawn).lycanthropeTypeDef.graphicData.drawSize;
                scaleFor = new Vector3(drawSize.x, 0, drawSize.y);
            }
            
            return scaleFor;
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
        private Vector3 scaleFor = Vector3.zero;
    }
}
