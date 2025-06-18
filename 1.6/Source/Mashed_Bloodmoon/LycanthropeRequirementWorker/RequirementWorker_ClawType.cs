using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    internal class RequirementWorker_ClawType : LycanthropeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            if (compLycanthrope.unlockedClawTracker.Contains(clawTypeDef))
            {
                return true;
            }
            return "Mashed_Bloodmoon_RequirementWorker_InvalidClawType".Translate(clawTypeDef.LabelCap);
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (clawTypeDef == null)
            {
                yield return "clawTypeDef is null";
            }
        }

        public LycanthropeClawTypeDef clawTypeDef;
    }
}
