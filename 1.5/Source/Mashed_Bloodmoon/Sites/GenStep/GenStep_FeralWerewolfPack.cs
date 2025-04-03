using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace Mashed_Bloodmoon
{
    public class GenStep_FeralWerewolfPack : GenStep
    {
        public FloatRange defaultPointsRange = new FloatRange(340f, 1000f);

        public override int SeedPart => 658435124;

        public override void Generate(Map map, GenStepParams parms)
        {
            if (!SiteGenStepUtility.TryFindRootToSpawnAroundRectOfInterest(out var rectToDefend, out var singleCellToSpawnNear, map))
            {
                return;
            }
            List<Pawn> list = new List<Pawn>();
            foreach (Pawn item in GeneratePawns(parms, map))
            {
                if (!SiteGenStepUtility.TryFindSpawnCellAroundOrNear(rectToDefend, singleCellToSpawnNear, map, out var spawnCell))
                {
                    Find.WorldPawns.PassToWorld(item);
                    break;
                }
                GenSpawn.Spawn(item, spawnCell, map);
                list.Add(item);
            }
            if (!list.Any())
            {
                return;
            }
            /*
            bool @bool = Rand.Bool;
            foreach (Pawn item2 in list)
            {
                CompWakeUpDormant comp = item2.GetComp<CompWakeUpDormant>();
                if (comp != null)
                {
                    comp.wakeUpIfTargetClose = @bool;
                }
            }
            */
            LordMaker.MakeNewLord(WerewolfUtility.GetFeralWerewolfFaction(), new LordJob_AssaultColony(WerewolfUtility.GetFeralWerewolfFaction()), map, list);
            //SendMechanoidsToSleepImmediately(list);
        }

        private IEnumerable<Pawn> GeneratePawns(GenStepParams parms, Map map)
        {
            float points = ((parms.sitePart != null) ? parms.sitePart.parms.threatPoints : defaultPointsRange.RandomInRange);
            PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
            {
                groupKind = PawnGroupKindDefOf.Combat,
                tile = map.Tile,
                faction = WerewolfUtility.GetFeralWerewolfFaction(),
                points = points
            };
            if (parms.sitePart != null)
            {
                pawnGroupMakerParms.seed = parms.sitePart.parms.randomValue;
            }
            return PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms);
        }
    }
}
