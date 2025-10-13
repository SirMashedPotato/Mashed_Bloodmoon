using RimWorld;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    [StaticConstructorOnStartup]

    public class Gizmo_LycanthropeStress : Gizmo_Slider
    {
        public HediffComp_LycanthropeTransformed transformedComp;
        private static bool draggingBar;
        private static float targetStress;

        public Gizmo_LycanthropeStress(HediffComp_LycanthropeTransformed comp)
        {
            transformedComp = comp;
            targetStress = transformedComp.StressMax;
        }

        protected override Color BarColor => new Color(0.7f, 0.3f, 0.3f);

        protected override Color BarHighlightColor => new Color(0.8f, 0.4f, 0.4f);

        protected override string BarLabel => transformedComp.currentStress + "/" + transformedComp.StressMax;

        protected override bool IsDraggable => false;

        protected override float Target
        {
            get
            {
                return targetStress;
            }
            set
            {
                targetStress = value;
            }
        }

        protected override float ValuePercent => transformedComp.currentStress / Mathf.Max(1f, transformedComp.StressMax);

        protected override string Title => "Mashed_Bloodmoon_LycanthropeStressGizmo_Label".Translate();

        protected override bool DraggingBar
        {
            get
            {
                return draggingBar;
            }
            set
            {
                draggingBar = value;
            }
        }

        protected override string GetTooltip() => "Mashed_Bloodmoon_LycanthropeStressGizmo_Tooltip".Translate(transformedComp.parent.pawn);
    }
}