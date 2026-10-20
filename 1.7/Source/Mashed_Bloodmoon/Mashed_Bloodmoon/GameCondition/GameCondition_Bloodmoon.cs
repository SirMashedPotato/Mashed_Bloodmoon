using RimWorld;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public abstract class GameCondition_Bloodmoon : GameCondition
    {
        public const float MaxSkyLerpFactor = 0.5f;
        public const float SkyGlow = 0.85f;
        public SkyColorSet BloodmoonColors = new SkyColorSet(new ColorInt(198, 65, 65).ToColor, new ColorInt(255, 200, 234).ToColor, new Color(0.8f, 0.6f, 0.5f), SkyGlow);

        public override bool AllowEnjoyableOutsideNow(Map map) => false;

        public override float AnimalDensityFactor(Map map) => 0;

        public override SkyTarget? SkyTarget(Map map) => new SkyTarget(SkyGlow, BloodmoonColors, 1f, 1f);
    }
}
