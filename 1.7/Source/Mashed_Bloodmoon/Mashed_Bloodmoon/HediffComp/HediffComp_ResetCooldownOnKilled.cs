using Verse;

namespace Mashed_Bloodmoon
{
    internal class HediffComp_ResetCooldownOnKilled : HediffComp
    {
        public HediffCompProperties_ResetCooldownOnKilled Props => (HediffCompProperties_ResetCooldownOnKilled)props;

        public override void Notify_PawnKilled()
        {
            HediffComp_Link compLink = parent.TryGetComp<HediffComp_Link>();
            if (compLink != null)
            {
                if (Props.beastHuntDef.Completed(compLink.OtherPawn))
                {
                    compLink.OtherPawn.abilities.GetAbility(Props.abilityDef).ResetCooldown();
                }
                else
                {
                    Props.beastHuntDef.ProgressBeastHunt(compLink.OtherPawn);
                }
            }
            base.Notify_PawnKilled();
        }
    }
}