using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Comp_DropOnDestroy : ThingComp
    {
        public CompProperties_DropOnDestroy Props => (CompProperties_DropOnDestroy)props;

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            if (Props.thingSetMakerDef != null)
            {
                for(int i = 0; i < Props.setMakerDrops; i++)
                {
                    List<Thing> setDropList = Props.thingSetMakerDef.root.Generate();
                    foreach (Thing thing in setDropList)
                    {
                        DropThing(thing, previousMap);
                    }
                }
            }
            if (!Props.guaranteedDrops.NullOrEmpty())
            {
                foreach (PotentialDrops drop in Props.guaranteedDrops)
                {
                    DoDrop(drop, previousMap);
                }
            }
            if (!Props.potentialDrops.NullOrEmpty())
            {
                PotentialDrops drop = Props.potentialDrops.RandomElementByWeight(x => x.weight);
                if (drop.thingDef != null)
                {
                    DoDrop(drop, previousMap);
                }
            }

            base.PostDestroy(mode, previousMap);
        }

        private void DoDrop(PotentialDrops drop, Map previousMap)
        {
            ThingDef stuff = null;
            if (!drop.thingDef.stuffCategories.NullOrEmpty())
            {
                stuff = GenStuff.RandomStuffByCommonalityFor(drop.thingDef);
            }
            Thing droppedThing = ThingMaker.MakeThing(drop.thingDef, stuff);
            droppedThing.stackCount = drop.amountRange.RandomInRange;
            droppedThing.TryGetComp<CompQuality>()?.SetQuality(QualityUtility.GenerateQualityRandomEqualChance(), ArtGenerationContext.Outsider);
            GenPlace.TryPlaceThing(droppedThing, parent.Position, previousMap, ThingPlaceMode.Near);
        }

        private void DropThing(Thing thing, Map previousMap)
        {
            GenPlace.TryPlaceThing(thing, parent.Position, previousMap, ThingPlaceMode.Near);
        }
    }
}
