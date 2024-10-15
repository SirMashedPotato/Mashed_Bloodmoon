using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LycanthropeTypeDef : Def
    {
        //public string defName;
        //public string label;
        public string artist = "???";
        public GraphicData graphicData;
        public LycanthropeTypeRequirementWorker requirementWorker;

        public class LycanthropeTypeRequirements
        {
            public List<ThingDef> raceDefs;
            public bool psylink = false;
        }

        public AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (requirementWorker != null)
            {
                return requirementWorker.PawnRequirementsMet(pawn);
            }
            return true;
        }

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
        }
    }
}
