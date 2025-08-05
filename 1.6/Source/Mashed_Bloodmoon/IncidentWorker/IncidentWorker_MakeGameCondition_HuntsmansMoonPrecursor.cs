using RimWorld;

namespace Mashed_Bloodmoon
{
    public class IncidentWorker_MakeGameCondition_HuntsmansMoonPrecursor : IncidentWorker_MakeGameCondition
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            return HuntsmansMoonUtility.GetValidIncidentDef(out IncidentDef _, parms) && base.CanFireNowSub(parms);
        }
    }
}
