using RimWorld;
using System.Linq;
using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class JobGiver_AttackMarkedTarget : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_SpectralBeast);
            if (hediff != null)
            {
                Pawn parentPawn = hediff.TryGetComp<HediffComp_SummonedBeast>().parentPawn;
                Hediff markedHediff = LycanthropeUtility.GetCompLycanthropeTransformed(parentPawn).linkedHediffs.LastOrDefault(x=>x.def == HediffDefOf.Mashed_Bloodmoon_HuntsmansMark);
                if (markedHediff != null && markedHediff.pawn != null && markedHediff.pawn.Map != null && markedHediff.pawn.Map == parentPawn.Map 
                    && !markedHediff.pawn.DeadOrDowned)
                {
                    return new Job(JobDefOf.AttackMelee, markedHediff.pawn);
                }
            }
            return null;
        }
    }
}
