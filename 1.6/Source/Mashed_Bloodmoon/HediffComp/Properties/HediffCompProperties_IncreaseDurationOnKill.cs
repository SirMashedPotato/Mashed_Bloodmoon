using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffCompProperties_IncreaseDurationOnKill : HediffCompProperties
    {
        public HediffCompProperties_IncreaseDurationOnKill() => compClass = typeof(HediffComp_IncreaseDurationOnKill);

        public LycanthropeBeastHuntDef beastHuntDef;
        public AbilityDef abilityDef;
        public int secondsPerKill = 1;

        public int TicksPerKill => GenTicks.SecondsToTicks(secondsPerKill);
    }
}
