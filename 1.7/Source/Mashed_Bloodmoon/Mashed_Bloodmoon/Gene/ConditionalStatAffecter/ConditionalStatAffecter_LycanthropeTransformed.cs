using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class ConditionalStatAffecter_LycanthropeTransformed : ConditionalStatAffecter
    {
        public override string Label => "Mashed_Bloodmoon_StatAffecter_LycanthropeTransformed".Translate();

        public override bool Applies(StatRequest req)
        {
            return ModsConfig.BiotechActive && req.HasThing && req.Thing is Pawn p && LycanthropeUtility.PawnIsTransformedLycanthrope(p);
        }
    }
}
