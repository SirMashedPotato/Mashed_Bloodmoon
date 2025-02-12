using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_AddLinkedHediff : CompAbilityEffect_GiveHediff
    {
        public new CompProperties_AbilityGiveHediff Props => (CompProperties_AbilityGiveHediff)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            if (target.Pawn != null)
            {
                Hediff hediff = target.Pawn.health.hediffSet.GetFirstHediffOfDef(Props.hediffDef);
                if (hediff != null)
                {
                    LycanthropeUtility.GetCompLycanthropeTransformed(parent.pawn).linkedHediffs.Add(hediff);
                }
            }
        }
    }
}
