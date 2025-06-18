using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffComp_LycanthropeTransformationEnd : HediffComp
    {
        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            base.CompPostTickInterval(ref severityAdjustment, delta);
            parent.pawn.health.RemoveHediff(parent);
        }

        /// <summary>
        /// Used to reset a lycanthropes work priorities, which are reset due to the transformed hediff disabling all work types
        /// Because this is triggered by CompPostTick it does not immediately take effect while the game is paused
        /// </summary>
        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            if (!LycanthropeUtility.PawnIsLycanthrope(parent.pawn))
            {
                return;
            }
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(parent.pawn);
            if (compLycanthrope.cachedWorkPriorities != null)
            {
                foreach (WorkTypeDef workTypeDef in DefDatabase<WorkTypeDef>.AllDefs)
                {
                    parent.pawn.workSettings.SetPriority(workTypeDef, compLycanthrope.cachedWorkPriorities[workTypeDef]);
                }
            }
            compLycanthrope.cachedWorkPriorities = null;
        }
    }
}
