using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class GreatBeastDef : Def
    {
        public ThingDef thingDef;

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }

            if (thingDef == null)
            {
                yield return "null thingDef";
            }
        }
    }
}
