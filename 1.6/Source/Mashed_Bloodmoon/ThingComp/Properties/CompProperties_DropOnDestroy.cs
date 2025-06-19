using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompProperties_DropOnDestroy : CompProperties
    {
        public CompProperties_DropOnDestroy() => compClass = typeof(Comp_DropOnDestroy);

        public List<PotentialDrops> guaranteedDrops;
        public List<PotentialDrops> potentialDrops;
        public ThingSetMakerDef thingSetMakerDef;
        public int setMakerDrops = 1;
    }

    public class PotentialDrops
    {
        public ThingDef thingDef;
        public IntRange amountRange;
        public float weight = 1;
    }
}
