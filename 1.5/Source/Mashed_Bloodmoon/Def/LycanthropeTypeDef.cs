using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LycanthropeTypeDef : Def
    {
        public string artist = "???";
        public LycanthropeGraphicData graphicData;
        private List<PawnRenderNodeProperties> renderNodeProperties;
        public LycanthropeTypeRequirementWorker requirementWorker;
        public LycanthropeTransformationWorker transformationWorker;

        public List<PawnRenderNodeProperties> RenderNodeProperties => renderNodeProperties ?? PawnRenderUtility.EmptyRenderNodeProperties;

        public AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (requirementWorker != null)
            {
                return requirementWorker.PawnRequirementsMet(pawn);
            }
            return true;
        }

        public override void ResolveReferences()
        {
            base.ResolveReferences();
            foreach(PawnRenderNodeProperties props in RenderNodeProperties)
            {
                props.ResolveReferences();
            }
        }

        public Color PrimaryColorDefault => graphicData.color;
        public Color SecondaryColorDefault => graphicData.colorTwo;
        public Color TertiaryColorDefault => graphicData.colorThree;

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }

            if (graphicData == null)
            {
                yield return "null graphicData";
            }

            if (requirementWorker != null)
            {
                foreach (string item in requirementWorker.ConfigErrors())
                {
                    yield return item;
                }
            }

            if (transformationWorker != null)
            {
                foreach (string item in transformationWorker.ConfigErrors())
                {
                    yield return item;
                }
            }
        }
    }
}
