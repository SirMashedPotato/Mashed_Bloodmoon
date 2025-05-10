using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffCompProperties_ResetCooldownOnKilled : HediffCompProperties
    {
        public HediffCompProperties_ResetCooldownOnKilled() => compClass = typeof(HediffComp_ResetCooldownOnKilled);

        public LycanthropeBeastHuntDef beastHuntDef;
        public AbilityDef abilityDef;
    }
}
