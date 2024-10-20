using RimWorld;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    [StaticConstructorOnStartup]

    public class Gizmo_LycanthropeStress : Gizmo
    {
        public override float GetWidth(float maxWidth)
        {
            return 140f;
        }

        public Gizmo_LycanthropeStress(HediffComp_LycanthropeTransformed comp)
        {
            transformedComp = comp;
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            Rect rect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            Rect rect2 = rect.ContractedBy(6f);
            Widgets.DrawWindowBackground(rect);
            Rect rect3 = rect2;
            rect3.height = rect.height / 2f;
            Text.Font = GameFont.Tiny;
            Widgets.Label(rect3, "Mashed_Bloodmoon_LycanthropeStressGizmo_Label".Translate());
            Rect rect4 = rect2;
            rect4.yMin = rect2.y + rect2.height / 2f;
            float fillPercent = transformedComp.currentStress / Mathf.Max(1f, transformedComp.StressMax);
            Widgets.FillableBar(rect4, fillPercent, FullBarTex, EmptyBarTex, false);
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(rect4, transformedComp.currentStress + "/" + transformedComp.StressMax);
            Text.Anchor = TextAnchor.UpperLeft;
            TooltipHandler.TipRegion(rect2, "Mashed_Bloodmoon_LycanthropeStressGizmo_Tooltip".Translate(transformedComp.parent.pawn));
            return new GizmoResult(GizmoState.Clear);
        }

        public HediffComp_LycanthropeTransformed transformedComp;
        private static readonly Texture2D FullBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.7f, 0.3f, 0.3f));
        private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);
    }
}