using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Page_GreatBeastHunt : LycanthropePage
    {
        public const int columnNumber = 5;
        private readonly List<GreatBeastDef> greatBeastList;
        private static Vector2 scrollPosition = Vector2.zero;

        public override string PageTitle => "Mashed_Bloodmoon_GreatBeastHunt".Translate().CapitalizeFirst() + ": " + pawn.NameShortColored;

        public Page_GreatBeastHunt(HediffComp_Lycanthrope comp) : base(comp)
        {
            greatBeastList = DefDatabase<GreatBeastDef>.AllDefsListForReading;
        }

        public override void DoWindowContents(Rect inRect)
        {
            DrawPageTitle(inRect);
            Widgets.ButtonImage(new Rect(inRect.width - 30f, 0f, 30f, 30f), TexButton.Info, true, "Mashed_Bloodmoon_GreatBeastHuntDesc".Translate(pawn));

            inRect.yMin += rectLimitY;
            DoBottomButtons(inRect, showNext: false);
            inRect.height -= rectLimitY;

            Rect scrollRect = inRect;
            Rect innerRect = scrollRect;
            innerRect.width -= 30f;

            float gridWidth = (innerRect.width / columnNumber) - rectPadding;
            float gridHeight = gridWidth * 1.5f;
            float rowCount = ((float)greatBeastList.Count / columnNumber) + 0.5f;
            innerRect.height = Mathf.Round(rowCount) * (gridHeight + rectPadding);

            Widgets.BeginScrollView(scrollRect, ref scrollPosition, innerRect);
            int row = 0;
            int column = 0;
            Rect greatBeastRect = new Rect(innerRect.x, innerRect.y, gridWidth, gridHeight);

            foreach (GreatBeastDef greatBeastDef in greatBeastList)
            {
                //DoGreatBeastGrid(greatBeastRect, greatBeastDef);
                Widgets.DrawMenuSection(greatBeastRect);
                Widgets.Label(greatBeastRect, greatBeastDef.label);

                if (++column >= columnNumber)
                {
                    greatBeastRect.y += ((rectPadding / 2f) + gridHeight);
                    greatBeastRect.x = innerRect.x;
                    column = 0;
                    row++;
                }
                else
                {
                    greatBeastRect.x += ((rectPadding / 2f) + gridWidth);
                }
            }
            Widgets.EndScrollView();
        }

        public void DoGreatBeastGrid(Rect inRect, GreatBeastDef greatBeastDef)
        {
            Rect rightRect = inRect;
            Rect leftRect = inRect;
            rightRect.width = rightRect.height;
            leftRect.width -= rightRect.width + (rectPadding / 2f);
            rightRect.x += leftRect.width + (rectPadding / 2f);
            
            DoGridLowerRect(leftRect, greatBeastDef);
            DoGridUpperRect(rightRect, greatBeastDef);
        }

        public void DoGridLowerRect(Rect inRect, GreatBeastDef greatBeastDef)
        {
            Widgets.DrawMenuSection(inRect);
            Rect detailsRect = inRect.ContractedBy(rectPadding);

            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(detailsRect);
            listing_Standard.Label(greatBeastDef.LabelCap);
            listing_Standard.Label(greatBeastDef.description);
            listing_Standard.End();
        }

        public void DoGridUpperRect(Rect inRect, GreatBeastDef greatBeastDef)
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
