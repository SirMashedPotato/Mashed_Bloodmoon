using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompletionWorker_UnlockAbility : LycanthropeBeastHuntCompletionWorker
    {
        public override void PostBeastHuntCompleted(HediffComp_Lycanthrope compLycanthrope, Pawn pawn)
        {
            abilityDef.Unlock(compLycanthrope);
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (abilityDef == null)
            {
                yield return "abilityDef is null";
            }
        }

        public LycanthropeAbilityDef abilityDef;
        public int level = 1;
    }
}
