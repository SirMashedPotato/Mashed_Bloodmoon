using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffComp_SaniesLupinus : HediffComp
    {
        public HediffCompProperties_SaniesLupinus Props => (HediffCompProperties_SaniesLupinus)props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (parent.Severity + severityAdjustment >= 1.0f && !parent.FullyImmune())
            {
                ///TODO letter etc
                parent.pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeDormant);
                parent.pawn.health.RemoveHediff(parent);
                return;
            }
            base.CompPostTick(ref severityAdjustment);
        }
    }
}
