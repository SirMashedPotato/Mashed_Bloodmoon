using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_AbilityProficiency : LycanthropeAbilityEffectComp
    {
        public new CompProperties_AbilityProficiency Props => (CompProperties_AbilityProficiency)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Props.beastHuntDef.ProgressBeastHunt(CompLycanthrope, parent.pawn);
            base.Apply(target, dest);
        }

        public override string ExtraTooltipPart()
        {
            return Props.beastHuntDef.Completed(CompLycanthrope) ? Props.completedTooltip : base.ExtraTooltipPart();
        }
    }
}
