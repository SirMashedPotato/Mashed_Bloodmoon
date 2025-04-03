using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    public class RitualAttachableOutcomeEffectWorker_GrantTotem : RitualAttachableOutcomeEffectWorker
    {
        private static List<LycanthropeTotemDef> validTotemDefs;

        public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            extraOutcomeDesc = null;
            validTotemDefs = DefDatabase<LycanthropeTotemDef>.AllDefs.Where(x=>x.canBePurchased).ToList();
            extraOutcomeDesc = IncreaseTotemLevels(totalPresence, outcome.BestPositiveOutcome(jobRitual));
        }

        private string IncreaseTotemLevels(Dictionary<Pawn, int> totalPresence, bool bestOutcome)
        {
            string extraDescription = "";
            int count = bestOutcome ? 2 : 1;
            foreach(Pawn pawn in totalPresence.Keys)
            {
                if (LycanthropeUtility.PawnIsLycanthrope(pawn))
                {
                    HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
                    for (int i = 0; i < count; i ++)
                    {
                        foreach (LycanthropeTotemDef totemDef in validTotemDefs.InRandomOrder())
                        {
                            if (totemDef.CanUpgrade(compLycanthrope))
                            {
                                extraDescription += pawn.NameShortColored + ": " + totemDef.LabelCap + " + 1\n";
                                totemDef.UseTotem(compLycanthrope, 1, false);
                                break;
                            }
                        }
                    }
                }
            }

            return extraDescription != "" ? extraDescription : null;
        }
    }
}
