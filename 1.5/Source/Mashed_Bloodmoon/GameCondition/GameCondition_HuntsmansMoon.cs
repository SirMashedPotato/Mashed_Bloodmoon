using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

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
            }
        }

        private void RandomizeTicksToNextRaid()
        {
            ticksToNextRaid += Rand.RangeInclusive(GenDate.TicksPerHour * 3, GenDate.TicksPerHour * 6); //TODO get values from setting
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
