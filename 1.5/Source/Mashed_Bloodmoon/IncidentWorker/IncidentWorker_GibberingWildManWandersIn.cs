using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class IncidentWorker_GibberingWildManWandersIn : IncidentWorker_WildManWandersIn
    {
        IntRange consumedHeartsRange = new IntRange(10,30);

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

            Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind: PawnKindDefOf.WildMan, context: PawnGenerationContext.NonPlayer, faction: formerFaction,
                mustBeCapableOfViolence: true,
                dontGiveWeapon: true,
                allowPregnant: false,
                allowFood: false));

            pawn.SetFaction(null);

            pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_Lycanthrope).Severity = 0.1f;
            LycanthropeUtility.UseTotem(pawn, TotemTypeDefOf.Mashed_Bloodmoon_ConsumedHearts, consumedHeartsRange.RandomInRange);

            GenSpawn.Spawn(pawn, cell, map);

            TaggedString taggedString = pawn.kindDef.GetLabelGendered(pawn.gender);
            TaggedString title = def.letterLabel.Formatted(taggedString, pawn.Named("PAWN")).CapitalizeFirst();
            TaggedString letterText = def.letterText.Formatted(pawn.NameShortColored, taggedString, pawn.Named("PAWN")).AdjustedFor(pawn).CapitalizeFirst();

            PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref letterText, ref title, pawn);
            SendStandardLetter(title, letterText, def.letterDef, parms, pawn);
            return true;
        }

        /// <summary>
        /// Copy pasted and not even edited :/
        /// </summary>
        private bool TryFindEntryCell(Map map, out IntVec3 cell)
        {
            return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Ignore, out cell);
        }

        /// <summary>
        /// Copy pasted and not even edited :/
        /// </summary>
        private bool TryFindFormerFaction(out Faction formerFaction)
        {
            return Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out formerFaction, tryMedievalOrBetter: false, allowDefeated: true);
        }
    }
}
