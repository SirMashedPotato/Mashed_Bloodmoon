using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    public abstract class LycanthropeBeastHuntCompletionWorker
    {
        public abstract void PostBeastHuntCompleted(HediffComp_Lycanthrope compLycanthrope, Pawn pawn);

        public virtual IEnumerable<string> ConfigErrors()
        {
            return Enumerable.Empty<string>();
        }
    }
}
