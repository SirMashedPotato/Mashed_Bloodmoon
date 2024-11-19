using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Page_GreatBeastHunt : LycanthropePage
    {
        public static readonly Vector2 PageSize = new Vector2(512f, 764f);
        public const float rowHeight = 128f;
        public float innerRectHeight;
        private List<GreatBeastDef> greatBeastList;
        private static Vector2 scrollPosition = Vector2.zero;

        public override string PageTitle => "Mashed_Bloodmoon_GreatBeastHunt".Translate().CapitalizeFirst() + ": " + pawn.NameShortColored;

        public Page_GreatBeastHunt(HediffComp_Lycanthrope comp) : base(comp)
        {
            greatBeastList = DefDatabase<GreatBeastDef>.AllDefsListForReading;
            innerRectHeight = greatBeastList.Count * (rowHeight + (rectPadding / 2f));
        }

        public override Vector2 InitialSize => PageSize;

        public override void DoWindowContents(Rect inRect)
        {
            DrawPageTitle(inRect);
            inRect.yMin += rectLimitY;
            DoBottomButtons(inRect, showNext: false);
            inRect.height -= rectLimitY;

            Rect scrollRect = inRect;
            Rect innerRect = scrollRect;
            innerRect.height = innerRectHeight;
            innerRect.width -= 30f;
            
            Widgets.BeginScrollView(scrollRect, ref scrollPosition, innerRect);
            int index = 0;
            foreach (GreatBeastDef greatBeastDef in greatBeastList)
            {
                Rect greatBeastRect = innerRect;
                greatBeastRect.height = rowHeight;
                greatBeastRect.y += (((rectPadding / 2f) + rowHeight) * index);
                DoGreatBeastRow(greatBeastRect);
                index++;
            }
            //Widgets.DrawMenuSection(innerRect);
            Widgets.EndScrollView();

        }

        public void DoGreatBeastRow(Rect inRect)
        {
            Widgets.DrawMenuSection(inRect);
        }
    }
}
