using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    public abstract class LycanthropeTypeRequirementWorker
    {
        public abstract AcceptanceReport PawnRequirementsMet(Pawn pawn);

        public virtual IEnumerable<string> ConfigErrors()
        {
            return Enumerable.Empty<string>();
        }

        public string DoMissingList<T>(List<T> list) 
        {
            string output = "Mashed_Bloodmoon_LTR_OneOf".Translate();
            foreach (T t in list) 
            {
                Def def = t as Def;
                output += " " + def.label + ",";
            }
            return output;
        }
    }
}
