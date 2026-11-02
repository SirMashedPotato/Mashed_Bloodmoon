using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_GiveHediffLinked : CompAbilityEffect_GiveHediff
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);

            if (target == null || target.Pawn == null) return;
            Hediff hediff = target.Pawn.health.hediffSet.GetFirstHediffOfDef(Props.hediffDef);
            if (hediff == null) return;
            HediffComp_Link hediffComp_Link = hediff.TryGetComp<HediffComp_Link>();
            hediffComp_Link?.drawConnection = true;
            hediffComp_Link?.other = parent.pawn;
            if (LycanthropeUtility.PawnIsTransformedLycanthrope(parent.pawn))
            {
                LycanthropeUtility.GetCompLycanthropeTransformed(parent.pawn).AddLinkedHediff(hediff);
            }
        }
    }
}
