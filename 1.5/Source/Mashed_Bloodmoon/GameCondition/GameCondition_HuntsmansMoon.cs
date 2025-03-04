using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class GameCondition_HuntsmansMoon : GameCondition
    {
        private bool bloodmoonBegun = false;
        private int ticksToNextRaid = 0;
        private const float MaxSkyLerpFactor = 0.5f;
        private const float SkyGlow = 0.85f;
        private SkyColorSet BloodmoonColors = new SkyColorSet(new ColorInt(198, 65, 65).ToColor, new ColorInt(255, 200, 234).ToColor, new Color(0.8f, 0.6f, 0.5f), SkyGlow);

        public override void Init()
        {
            RandomizeTicksToNextRaid();
        }

        public override void GameConditionTick()
        {
            base.GameConditionTick();

            if (TicksPassed > TransitionTicks)
            {
                if (!bloodmoonBegun)
                {
                    Log.Message("initial actions");
                    DoInitialActions();
                    return;
                }
                if (TicksPassed >= ticksToNextRaid)
                {
                    Log.Message("ticks reached");
                    RandomizeTicksToNextRaid();
                }
            }
        }

        private void DoInitialActions()
        {
            bloodmoonBegun = true;
        }

        private void RandomizeTicksToNextRaid()
        {
            ticksToNextRaid += Rand.RangeInclusive(GenDate.TicksPerHour * 3, GenDate.TicksPerHour * 6); //TODO get values from setting
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref bloodmoonBegun, "bloodmoonBegun", false);
            Scribe_Values.Look(ref ticksToNextRaid, "ticksToNextRaid", 0);
        }

        public override bool AllowEnjoyableOutsideNow(Map map) => false;

        public override string TooltipString => base.TooltipString;

        public override int TransitionTicks => 600;

        public override SkyTarget? SkyTarget(Map map) => new SkyTarget(SkyGlow, BloodmoonColors, 1f, 1f);

        public override float SkyTargetLerpFactor(Map map) => GameConditionUtility.LerpInOutValue(this, TransitionTicks, MaxSkyLerpFactor);
    }
}
