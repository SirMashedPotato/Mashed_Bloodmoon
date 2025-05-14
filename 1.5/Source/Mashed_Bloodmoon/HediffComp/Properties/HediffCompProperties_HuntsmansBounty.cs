using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffCompProperties_HuntsmansBounty : HediffCompProperties
    {
        public HediffCompProperties_HuntsmansBounty() => compClass = typeof(HediffComp_HuntsmansBounty);

        public LycanthropeBeastHuntDef beastHuntDef;
        public ThingDef rewardThingDef;
        public float rewardFactor = 0.15f;
        public float extraRewardFactor = 0.1f;
    }
}
