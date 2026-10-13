using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace Mashed_Bloodmoon
{
    public class IncidentWorker_Ambush_FeralWerewolves : IncidentWorker_Ambush
    {
        protected override bool CanFireNowSub(IncidentParms parms)
        {
            return false;
        }

        protected override List<Pawn> GeneratePawns(IncidentParms parms)
        {
            PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, parms);
            return PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms).ToList();
        }

        protected override LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
        {
            return new LordJob_AssaultColony(parms.faction);
        }

        protected override string GetLetterText(Pawn anyPawn, IncidentParms parms)
        {
            return def.letterText.Formatted((parms.target is Caravan caravan) ? caravan.Name : "yourCaravan".TranslateSimple(), parms.faction.def.pawnsPlural, parms.faction.NameColored).Resolve().CapitalizeFirst();
        }
    }
}
