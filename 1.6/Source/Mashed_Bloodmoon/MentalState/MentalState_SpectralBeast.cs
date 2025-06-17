using RimWorld;
using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class MentalState_SpectralBeast : MentalState
    {
        public override bool ForceHostileTo(Faction f)
        {
            return f.HostileTo(sourceFaction);
        }

        public override bool ForceHostileTo(Thing t)
        {
            return t.HostileTo(sourceFaction);
        }
    }
}
