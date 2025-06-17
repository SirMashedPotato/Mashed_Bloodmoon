using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_FleckOnTargetExt : CompAbilityEffect_FleckOnTarget
    {
        public new CompProperties_AbilityFleckOnTargetExt Props => (CompProperties_AbilityFleckOnTargetExt)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (Props.onlySelf && (target.Pawn == null || target.Pawn != parent.pawn))
            {
                return;
            }

            base.Apply(target, dest);
        }
    }
}
