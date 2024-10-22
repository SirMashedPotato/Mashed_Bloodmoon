using Verse;

namespace Mashed_Bloodmoon
{
    public class Gene_Lycanthrope : Gene
    {
        public override void PostAdd()
        {
            base.PostAdd();
            if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_Lycanthrope) == null)
            {
                Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_SaniesLupinus);
                if (hediff != null)
                {
                    pawn.health.RemoveHediff(hediff);
                }

                hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeDormant);
                if (hediff != null)
                {
                    pawn.health.RemoveHediff(hediff);
                }

                pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_Lycanthrope);
            }
        }
    }
}