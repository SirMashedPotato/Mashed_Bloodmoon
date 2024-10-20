﻿using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTTWorker_AddAbility : LycanthropeTypeTransformationWorker
    {
        public override void PostTransformationBegin(Pawn pawn)
        {
            if (abilityDef != null)
            {
                pawn.abilities.GainAbility(abilityDef);
            }
        }

        public override void PostTransformationEnd(Pawn pawn)
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
