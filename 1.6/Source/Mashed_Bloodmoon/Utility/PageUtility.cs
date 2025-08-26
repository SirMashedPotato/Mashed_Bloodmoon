using Verse;

namespace Mashed_Bloodmoon
{
    internal static class PageUtility
    {
        internal static void ModSourceIcon(ref RectDivider rectDivider, Def def, HorizontalJustification justification = HorizontalJustification.Right)
        {
            string tooltip = "Source".Translate() + ": " + def.modContentPack.ModMetaData.Name;
            Widgets.ButtonImage(rectDivider.NewCol(rectDivider.Rect.height, justification), def.modContentPack.ModMetaData.Icon, tooltip: tooltip);
        }
    }
}
