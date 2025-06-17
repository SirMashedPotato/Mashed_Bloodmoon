using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffComp_HuntsmansBounty : HediffComp
    {
        public HediffCompProperties_HuntsmansBounty Props => (HediffCompProperties_HuntsmansBounty)props;

        public override void Notify_PawnKilled()
        {
            float finalRewardFactor = Props.rewardFactor;
            HediffComp_Link compLink = parent.TryGetComp<HediffComp_Link>();
            if (compLink != null)
            {
                if (Props.beastHuntDef.Completed(compLink.OtherPawn))
                {
                    finalRewardFactor += Props.extraRewardFactor;
                }
                else
                {
                    Props.beastHuntDef.ProgressBeastHunt(compLink.OtherPawn);
                }
            }

            Thing thing = ThingMaker.MakeThing(Props.rewardThingDef);
            thing.stackCount = (int)(parent.pawn.kindDef.combatPower * finalRewardFactor);
            GenPlace.TryPlaceThing(thing, parent.pawn.Position, parent.pawn.Map, ThingPlaceMode.Near);

            base.Notify_PawnKilled();
        }
    }
}