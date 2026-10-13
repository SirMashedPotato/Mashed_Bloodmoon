using RimWorld;

namespace Mashed_Bloodmoon
{
    public class IncidentWorker_MakeGameCondition_HuntsmanMoon : IncidentWorker_MakeGameCondition
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            return Mashed_Bloodmoon_ModSettings.HuntsmanMoon_EnableCondition && base.CanFireNowSub(parms);
        }
    }
}
