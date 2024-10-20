using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    public abstract class LycanthropeTypeTransformationWorker
    {
        public abstract void PostTransformationBegin(Pawn pawn);

        public abstract void PostTransformationEnd(Pawn pawn);

        public virtual IEnumerable<string> ConfigErrors()
        {
            return Enumerable.Empty<string>();
        }
    }
}
