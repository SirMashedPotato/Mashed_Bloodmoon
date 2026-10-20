using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class GenStep_FeralWerewolfAmbush_Edge : GenStep_Ambush_Edge
    {
        public override int SeedPart => 610406664;

        protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
        {
            SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root, parms);
            signalAction_Ambush.ambushType = SignalActionAmbushType.Normal;
            return signalAction_Ambush;
        }
    }
}
