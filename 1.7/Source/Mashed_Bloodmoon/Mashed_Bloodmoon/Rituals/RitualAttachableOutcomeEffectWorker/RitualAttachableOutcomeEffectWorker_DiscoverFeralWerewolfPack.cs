using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class RitualAttachableOutcomeEffectWorker_DiscoverFeralWerewolfPack : RitualAttachableOutcomeEffectWorker
    {
        public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            WerewolfUtility.GenerateWerewolfPackQuest(out Quest quest);
            extraOutcomeDesc = quest.description;
            if (outcome.BestPositiveOutcome(jobRitual) && Rand.Chance(0.5f))
            {
                WerewolfUtility.GenerateWerewolfPackQuest(out _);
                extraOutcomeDesc += " + 1";
            }
        }
    }
}
