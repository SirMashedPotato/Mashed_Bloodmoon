using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_EffecterOnTargetExt : CompAbilityEffect_EffecterOnTarget
    {
        public new CompProperties_AbilityEffecterOnTargetExt Props => (CompProperties_AbilityEffecterOnTargetExt)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (Props.onlySelf && (target.Pawn == null || target.Pawn != parent.pawn))
            {
                return;
            }

            if (Props.onlyHostile && (target.Pawn == null || !target.Pawn.HostileTo(parent.pawn)))
            {
                return;
            }

            base.Apply(target, dest);
        }
    }
}
