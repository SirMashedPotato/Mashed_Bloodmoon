using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace Mashed_Bloodmoon
{
    public class GameCondition_HuntsmansMoon : GameCondition
    {
        private int ticksToNextRaid = 0;
        private const float MaxSkyLerpFactor = 0.5f;
        private const float SkyGlow = 0.85f;
        private SkyColorSet BloodmoonColors = new SkyColorSet(new ColorInt(198, 65, 65).ToColor, new ColorInt(255, 200, 234).ToColor, new Color(0.8f, 0.6f, 0.5f), SkyGlow);

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
            ticksToNextRaid += Rand.RangeInclusive(GenDate.TicksPerHour * 4, GenDate.TicksPerHour * 7); //TODO get values from setting
        }

        private void ApplyHuntsmanMoonTransformation()
        {
            foreach(Map map in AffectedMaps)
            {
                List<Pawn> pawns = map.mapPawns.AllHumanlikeSpawned;
                foreach(Pawn pawn in pawns)
                {
                    if (pawn.health.hediffSet.HasHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeDormant))
                    {
                        if (!pawn.health.hediffSet.HasHediff(HediffDefOf.Mashed_Bloodmoon_WolfsbanePrevention))
                        {
                            LycanthropeUtility.ApplyImminentTransformation(pawn, TransitionTicks);
                        }
                    }
                }
            }
        }

        private void TriggerWerewolfAttack()
        {
            int caravanCount = Find.WorldObjects.CaravansCount;
            List<Map> playerMaps = Find.Maps.Where(x => x.IncidentTargetTags().Contains(IncidentTargetTagDefOf.Map_PlayerHome)).ToList();
            if (caravanCount > 0 && Rand.RangeInclusive(0, caravanCount + playerMaps.Count()) < caravanCount - 1)
            {
                TriggerWerewolfAmbush();
                return;
            }
            if (!playerMaps.NullOrEmpty())
            {
                TriggerWerewolfRaid(playerMaps);
                return;
            }
        }

        private void TriggerWerewolfAmbush()
        {
            IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentDefOf.Mashed_Bloodmoon_WerewolfAmbush.category, Find.WorldObjects.Caravans.RandomElement());
            incidentParms.forced = true;
            incidentParms.faction = WerewolfUtility.GetFeralWerewolfFaction();
            IncidentDef incidentDef = IncidentDefOf.Mashed_Bloodmoon_WerewolfAmbush;
            if (incidentParms.points < 200)
            {
                incidentParms.points = 200;
            }
            incidentDef.Worker.TryExecute(incidentParms);
        }

        private void TriggerWerewolfRaid(List<Map> possibleMaps)
        {
            IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(RimWorld.IncidentDefOf.RaidEnemy.category, possibleMaps.RandomElement());
            incidentParms.forced = true;
            incidentParms.faction = WerewolfUtility.GetFeralWerewolfFaction();
            incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
            incidentParms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkInGroups;
            if (incidentParms.points < 200)
            {
                incidentParms.points = 200;
            }
            IncidentDef incidentDef = RimWorld.IncidentDefOf.RaidEnemy;
            incidentDef.Worker.TryExecute(incidentParms);
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

        public override bool AllowEnjoyableOutsideNow(Map map) => false;

        public override string TooltipString => base.TooltipString;

        public override int TransitionTicks => 600;

        public override SkyTarget? SkyTarget(Map map) => new SkyTarget(SkyGlow, BloodmoonColors, 1f, 1f);

        public override float SkyTargetLerpFactor(Map map) => GameConditionUtility.LerpInOutValue(this, TransitionTicks, MaxSkyLerpFactor);
    }
}
