using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    [StaticConstructorOnStartup]
    public class Page_GreatBeastHunt : LycanthropePage
    {
        public static readonly Vector2 PageSize = new Vector2(512f, 764f);
        public const float rowHeight = 90f;
        public float innerRectHeight;
        private readonly List<GreatBeastDef> greatBeastList;
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
            Widgets.ButtonImage(new Rect(inRect.width - 30f, 0f, 30f, 30f), TexButton.Info, true, "Mashed_Bloodmoon_GreatBeastHuntDesc".Translate(pawn));

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
                DoGreatBeastRow(greatBeastRect, greatBeastDef);
                index++;
            }
            Widgets.EndScrollView();
        }

        public void DoGreatBeastRow(Rect inRect, GreatBeastDef greatBeastDef)
        {
            Rect rightRect = inRect;
            Rect leftRect = inRect;
            rightRect.width = rightRect.height;
            leftRect.width -= rightRect.width + (rectPadding / 2f);
            rightRect.x += leftRect.width + (rectPadding / 2f);

            DoRowLeftRect(leftRect, greatBeastDef);
            DoRowRightRect(rightRect, greatBeastDef);
        }

        public void DoRowLeftRect(Rect inRect, GreatBeastDef greatBeastDef)
        {
            Widgets.DrawMenuSection(inRect);
            Rect detailsRect = inRect.ContractedBy(rectPadding);

            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(detailsRect);
            listing_Standard.Label(greatBeastDef.LabelCap);
            listing_Standard.Label(greatBeastDef.description);
            listing_Standard.End();
        }

        public void DoRowRightRect(Rect inRect, GreatBeastDef greatBeastDef)
        {
            Widgets.DrawMenuSection(inRect);
            GUI.DrawTexture(inRect, ContentFinder<Texture2D>.Get(greatBeastDef.heartTexPath));
            if (compLycanthrope.greatBeastHeartTracker.Contains(greatBeastDef))
            {
                GUI.DrawTexture(inRect, ContentFinder<Texture2D>.Get(greatBeastDef.consumedTexPath));
            }
        }
    }
}
