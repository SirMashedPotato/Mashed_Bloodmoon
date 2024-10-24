using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class TotemTypeDef : Def
    {
        public ThingDef totemThingDef;
        public StatDef statDef;
        public int useLimit = 30;
        public float increasePerLevel = 1f;
        public bool onlyTransformed = true;
        public LycanthropeTypeTransformationWorker transformationWorker;

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }

            if (totemThingDef == null)
            {
                yield return "null totemThingDef";
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
