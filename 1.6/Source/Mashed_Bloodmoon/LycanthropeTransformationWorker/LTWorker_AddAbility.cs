using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTWorker_AddAbility : LycanthropeTransformationWorker
    {
        public override void PostTransformationBegin(Pawn pawn, int value = 0)
        {
            if (abilityDef != null)
            {
                pawn.abilities.GainAbility(abilityDef);
            }
        }

        public override void PostTransformationEnd(Pawn pawn, int value = 0)
        {
            if (abilityDef != null)
            {
                pawn.abilities.RemoveAbility(abilityDef);
            }
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (abilityDef == null)
            {
                yield return "abilityDef is null";
            }
        }

        public AbilityDef abilityDef;
    }
}
