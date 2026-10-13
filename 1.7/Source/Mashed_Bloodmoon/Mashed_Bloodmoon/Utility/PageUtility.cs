using RimWorld;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    internal static class PageUtility
    {
        internal static void ModSourceIcon(ref RectDivider rectDivider, Def def, HorizontalJustification justification = HorizontalJustification.Right)
        {
            string tooltip = "Source".Translate() + ": " + def.modContentPack.ModMetaData.Name;
            if (Widgets.ButtonImage(rectDivider.NewCol(rectDivider.Rect.height, justification), def.modContentPack.ModMetaData.Icon, tooltip: tooltip))
            {
                InfoButtonPopup(tooltip);
            }
        }

        internal static void InfoButton(ref RectDivider rectDivider, string description, HorizontalJustification justification = HorizontalJustification.Right)
        {
            InfoButton(rectDivider.NewCol(rectDivider.Rect.height, justification), TexButton.Info, description);
        }

        internal static void InfoButton(Rect rect, string description)
        {
            InfoButton(rect, TexButton.Info, description);
        }

        internal static void InfoButton(Rect rect, Texture2D tex, string description)
        {
            if (Widgets.ButtonImage(rect, tex, false, description))
            {
                InfoButtonPopup(description);
            }
        }

        internal static void InfoButtonPopup(string description)
        {
            Dialog_InfoButtonPopup popup = new Dialog_InfoButtonPopup(description);
            Find.WindowStack.Add(popup);
        }
    }
}
