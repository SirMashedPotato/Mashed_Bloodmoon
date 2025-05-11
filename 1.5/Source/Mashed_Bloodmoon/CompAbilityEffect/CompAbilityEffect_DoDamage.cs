using Verse;
using RimWorld;

namespace Mashed_Bloodmoon
{
    class CompAbilityEffect_DoDamage : CompAbilityEffect
    {
        public new CompProperties_AbilityDoDamage Props => (CompProperties_AbilityDoDamage)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (Props.damageDef != null)
            {
                if (target.Pawn != null)
                {
                    if (Props.onlyHostile && !target.Pawn.HostileTo(parent.pawn))
                    {
                        return;
                    }
                    DamageInfo damageInfo = new DamageInfo
                    {
                        Def = Props.damageDef
                    };
                    damageInfo.SetAmount(Props.damageAmount);
                    target.Pawn.TakeDamage(damageInfo);

                    if (Props.beastHuntDef != null)
                    {
                        if (Props.beastHuntDef.Completed(parent.pawn) && Props.extraHediffDef != null)
                        {
                            Hediff hediff = HediffMaker.MakeHediff(Props.extraHediffDef, target.Pawn);
                            HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
                            if (hediffComp_Disappears != null)
                            {
                                hediffComp_Disappears.ticksToDisappear = Props.extraHediffDuration.SecondsToTicks();
                            }
                            target.Pawn.health.AddHediff(hediff);
                        }
                        else
                        {
                            Props.beastHuntDef.ProgressBeastHunt(parent.pawn);
                        }
                    }
                }
            }
        }
    }
}