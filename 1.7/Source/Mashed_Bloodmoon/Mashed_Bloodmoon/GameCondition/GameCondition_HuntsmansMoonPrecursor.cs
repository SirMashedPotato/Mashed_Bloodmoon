using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class GameCondition_HuntsmansMoonPrecursor : GameCondition
    {
        public override void End()
        {
            IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Mashed_Bloodmoon_HuntsmansMoon, Find.World);
            if (HuntsmansMoonUtility.GetValidIncidentDef(out IncidentDef incidentDef, parms))
            {
                incidentDef.Worker.TryExecute(parms);
            }
            else
            {
                Messages.Message("Mashed_Bloodmoon_HuntsmansMoonPrecursorPasses".Translate(), MessageTypeDefOf.NeutralEvent, false);
            }
            base.End();
        }

        public override bool AllowEnjoyableOutsideNow(Map map) => false;

        public override float AnimalDensityFactor(Map map) => 0;

        public override int TransitionTicks => Duration;
    }
}
