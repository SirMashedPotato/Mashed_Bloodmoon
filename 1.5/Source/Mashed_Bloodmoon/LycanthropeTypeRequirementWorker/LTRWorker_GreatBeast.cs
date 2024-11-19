using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    internal class LTRWorker_GreatBeast : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            if (compLycanthrope.greatBeastHeartTracker.Contains(greatBeastDef))
            {
                return true;
            }
            return "Mashed_Bloodmoon_LTR_InvalidGreatBeast".Translate(greatBeastDef);
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (greatBeastDef == null)
            {
                yield return "greatBeastDef is null";
            }
        }

        public GreatBeastDef greatBeastDef;
    }
}
