using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompletionWorker_UpgradeTotem : LycanthropeBeastHuntCompletionWorker
    {
        public override void PostBeastHuntCompleted(HediffComp_Lycanthrope compLycanthrope, Pawn pawn)
        {
            totemTypeDef.Upgrade(compLycanthrope, useCount);
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (totemTypeDef == null)
            {
                yield return "totemTypeDef is null";
            }
        }

        public LycanthropeTotemDef totemTypeDef;
        public int useCount = 1;
    }
}
