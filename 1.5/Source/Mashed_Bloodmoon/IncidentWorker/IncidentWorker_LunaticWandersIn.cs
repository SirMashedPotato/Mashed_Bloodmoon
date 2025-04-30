using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class IncidentWorker_LunaticWandersIn : IncidentWorker_WildManWandersIn
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            if (!TryFindEntryCell(map, out var cell))
            {
                return false;
            }
            if (!TryFindFormerFaction(out var formerFaction))
            {
                return false;
            }

            Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.WildMan, formerFaction, 
                mustBeCapableOfViolence: true, 
                forcedTraits: new List<TraitDef> { TraitDefOf.Mashed_Bloodmoon_UncontrollableLycanthropy }
                ));
            pawn.SetFaction(null);
            AddLycanthropy(pawn);
            GenSpawn.Spawn(pawn, cell, map);

            TaggedString title = def.letterLabel.Formatted(pawn.KindLabel, pawn.Named("PAWN")).CapitalizeFirst();
            TaggedString text2 = def.letterText.Formatted(pawn.NameShortColored, pawn.KindLabel, pawn.Named("PAWN")).AdjustedFor(pawn).CapitalizeFirst();
            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text2, ref title, pawn);
            SendStandardLetter(title, text2, def.letterDef, parms, pawn);
            return true;
        }

        private void AddLycanthropy(Pawn pawn)
        {
            PawnLycanthropeProperties props = PawnLycanthropeProperties.Get(def);
            props.FillCompLycanthrope(pawn);
        }

        private bool TryFindEntryCell(Map map, out IntVec3 cell)
        {
            return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Ignore, out cell);
        }

        private bool TryFindFormerFaction(out Faction formerFaction)
        {
            return Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out formerFaction, tryMedievalOrBetter: false, allowDefeated: true);
        }
    }
}
