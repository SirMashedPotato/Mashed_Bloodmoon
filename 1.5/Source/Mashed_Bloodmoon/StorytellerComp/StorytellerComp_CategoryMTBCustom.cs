using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class StorytellerComp_CategoryMTBCustom : StorytellerComp
    {
        public StorytellerCompProperties_CategoryMTBCustom Props => (StorytellerCompProperties_CategoryMTBCustom)props;

        public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
        {
            float num = Props.mtbDays; //TODO switch to value in settings

            if (Rand.MTBEventOccurs(num, GenDate.TicksPerDay, GenTicks.TickLongInterval))
            {
                IncidentParms parms = GenerateParms(Props.category, target);
                if (TrySelectRandomIncident(UsableIncidentsInCategory(Props.category, parms), out var foundDef, target))
                {
                    yield return new FiringIncident(foundDef, this, parms);
                }
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()} {Props.category}";
        }
    }
}
