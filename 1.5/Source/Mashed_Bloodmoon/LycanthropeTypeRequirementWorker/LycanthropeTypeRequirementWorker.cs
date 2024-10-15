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
    }
}
