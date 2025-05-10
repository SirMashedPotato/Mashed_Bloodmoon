using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffComp_IncreaseDurationOnKill : HediffComp
    {
        public HediffCompProperties_IncreaseDurationOnKill Props => (HediffCompProperties_IncreaseDurationOnKill)props;

        public override void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
        {
            if (Props.beastHuntDef.Completed(parent.pawn))
            {
                CompDisappears.ticksToDisappear += Props.TicksPerKill;
                Ability ability = parent.pawn.abilities.GetAbility(Props.abilityDef);
                ability.StartCooldown(ability.CooldownTicksRemaining + Props.TicksPerKill);
            }
            base.Notify_KilledPawn(victim, dinfo);
        }

        private HediffComp_Disappears compDisappears;

        private HediffComp_Disappears CompDisappears
        {
            get
            {
                if (compDisappears == null)
                {
                    compDisappears = parent.TryGetComp<HediffComp_Disappears>();
                }
                return compDisappears;
            }
        }
    }
}
