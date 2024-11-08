﻿using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTRWorker_Skill : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (pawn.skills.GetSkill(skillDef).Level >= skillLevel)
            {
                return true;
            }
            return "Mashed_Bloodmoon_LTR_InvalidSkill".Translate(skillDef, skillLevel);
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (skillDef == null)
            {
                yield return "skillDef is null";
            }
        }

        public SkillDef skillDef;
        public int skillLevel = 1;
    }
}
