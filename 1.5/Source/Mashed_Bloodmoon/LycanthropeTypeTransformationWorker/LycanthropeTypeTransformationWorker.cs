using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    public abstract class LycanthropeTypeTransformationWorker
    {
        public abstract void PostTransformationBegin(Pawn pawn, int value = 0);

        public abstract void PostTransformationEnd(Pawn pawn, int value = 0);

        public virtual IEnumerable<string> ConfigErrors()
        {
            return Enumerable.Empty<string>();
        }
    }
}
