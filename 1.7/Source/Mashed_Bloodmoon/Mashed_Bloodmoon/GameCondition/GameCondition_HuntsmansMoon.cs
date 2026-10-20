using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace Mashed_Bloodmoon
{
    public class GameCondition_HuntsmansMoon : GameCondition_Bloodmoon
    {
        private int ticksToNextRaid = 0;

        public override void Init()
        {
            RandomizeTicksToNextRaid();
            ApplyHuntsmanMoonTransformation();
        }

        public override void GameConditionTick()
        {
            base.GameConditionTick();

            if (TicksPassed >= ticksToNextRaid)
            {
                RandomizeTicksToNextRaid();
                TriggerWerewolfAttack();
            }
        }

        private void RandomizeTicksToNextRaid()
        {
            ticksToNextRaid += (Mashed_Bloodmoon_ModSettings.HuntsmanMoon_HoursBetweenAttacks.RandomInRange * GenDate.TicksPerHour);
        }

        private void ApplyHuntsmanMoonTransformation()
        {
            foreach(Map map in AffectedMaps)
            {
                List<Pawn> pawns = map.mapPawns.AllHumanlikeSpawned;
                foreach(Pawn pawn in pawns)
                {
                    if (!pawn.health.hediffSet.HasHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeDormant))
                    {
                        continue;
                    }
                    if (pawn.health.hediffSet.HasHediff(HediffDefOf.Mashed_Bloodmoon_WolfsbanePrevention))
                    {
                        continue;
                    }
                    if (pawn.Downed || pawn.Deathresting)
                    {
                        continue;
                    }
                    TransformationUtility.ApplyImminentTransformation(pawn, TransitionTicks);
                }
            }
        }

        private void TriggerWerewolfAttack()
        {
            List<Caravan> playerCaravans = Find.WorldObjects.Caravans.Where(x => FactionDefOf.Mashed_Bloodmoon_FeralWerewolves.layerWhitelist.Contains(x.Tile.LayerDef)).ToList();
            List<Map> playerMaps = Find.Maps.Where(x => x.IncidentTargetTags().Contains(IncidentTargetTagDefOf.Map_PlayerHome) && 
            FactionDefOf.Mashed_Bloodmoon_FeralWerewolves.layerWhitelist.Contains(x.Tile.LayerDef)).ToList();
            float pointsMult = Mashed_Bloodmoon_ModSettings.HuntsmanMoon_RaidPointsMultiplier;
            if (!playerCaravans.NullOrEmpty() && Rand.RangeInclusive(0, playerCaravans.Count + playerMaps.Count()) < playerCaravans.Count - 1)
            {
                TriggerWerewolfAmbush(playerCaravans);
                return;
            }
            if (!playerMaps.NullOrEmpty())
            {
                TriggerWerewolfRaid(playerMaps);
                return;
            }
        }

        private void TriggerWerewolfAmbush(List<Caravan> possibleCaravans)
        {
            WerewolfUtility.TriggerWerewolfAmbush(possibleCaravans);
        }

        private void TriggerWerewolfRaid(List<Map> possibleMaps)
        {
            WerewolfUtility.TriggerWerewolfRaid(possibleMaps.RandomElement());
        }

        public override void End()
        {
            foreach (Map map in AffectedMaps)
            {
                EndWerewolfRaids(map);
            }

            WerewolfUtility.GenerateWerewolfPackQuest(out _);

            base.End();
        }

        private void EndWerewolfRaids(Map map)
        {
            List<Lord> lords = map.lordManager.lords.FindAll(x => x.faction == WerewolfUtility.GetFeralWerewolfFaction()).ToList();
            foreach (Lord lord in lords)
            {
                LordToil_ExitMap lordToil = new LordToil_ExitMap(LocomotionUrgency.Sprint, true, true)
                {
                    lord = lord,
                    useAvoidGrid = true,
                };

                lord.GotoToil(lordToil);
            }
        }


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ticksToNextRaid, "ticksToNextRaid", 0);
        }

        public override int TransitionTicks => 600;

        public override float SkyTargetLerpFactor(Map map) => GameConditionUtility.LerpInOutValue(this, TransitionTicks, MaxSkyLerpFactor);
    }
}
