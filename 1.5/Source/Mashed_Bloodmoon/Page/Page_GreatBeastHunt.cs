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
            float gridHeight = gridWidth * 1.6f;
            float rowCount = ((float)greatBeastList.Count / columnNumber) + 0.5f;
            innerRect.height = Mathf.Round(rowCount) * (gridHeight + rectPadding);

            Widgets.BeginScrollView(scrollRect, ref scrollPosition, innerRect);
            int row = 0;
            int column = 0;
            Rect greatBeastRect = new Rect(innerRect.x, innerRect.y, gridWidth, gridHeight);

            foreach (GreatBeastDef greatBeastDef in greatBeastList)
            {
                DoGreatBeastGrid(greatBeastRect, greatBeastDef);
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
            Widgets.DrawMenuSection(inRect);
            Rect upperRect = inRect;
            Rect lowerRect = inRect;
            upperRect.height = upperRect.width;
            lowerRect.height -= upperRect.height + (rectPadding / 2f);
            lowerRect.y += upperRect.height + (rectPadding / 2f);
            
            DoGridUpperRect(upperRect, greatBeastDef);
            DoGridLowerRect(lowerRect, greatBeastDef);
        }

        public void DoGridUpperRect(Rect inRect, GreatBeastDef greatBeastDef)
        {
            Rect mainRect = inRect.ContractedBy(rectPadding);
            GUI.DrawTexture(mainRect, ContentFinder<Texture2D>.Get(greatBeastDef.backgroundTexPath));
            GUI.DrawTexture(mainRect, ContentFinder<Texture2D>.Get(greatBeastDef.heartTexPath));
            if (greatBeastDef.Completed(compLycanthrope))
            {
                GUI.DrawTexture(mainRect, ContentFinder<Texture2D>.Get(greatBeastDef.consumedTexPath));
            }
            else
            {
                if (greatBeastDef.consumeCount > 1)
                {
                    TaggedString label = greatBeastDef.Progress(compLycanthrope) + "/" + greatBeastDef.consumeCount;
                    Rect progressRect = mainRect.ContractedBy(rectPadding);
                    progressRect.height = Text.LineHeight;
                    progressRect.y += mainRect.height - (progressRect.height + (rectPadding * 1.5f));
                    Widgets.Label(progressRect, label);
                }
            }
        }

        public void DoGridLowerRect(Rect inRect, GreatBeastDef greatBeastDef)
        {
            Rect detailsRect = inRect.ContractedBy(rectPadding);
            TaggedString label = greatBeastDef.LabelCap;
            detailsRect.SplitHorizontally(detailsRect.height / 3f, out Rect upperRect, out Rect lowerRect);
            var anchor = Text.Anchor;
            var font = Text.Font;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.LabelFit(upperRect, label);
            Text.Font = GameFont.Tiny;
            Widgets.Label(lowerRect, greatBeastDef.description);
            Text.Anchor = anchor;
            Text.Font = font;

        }
    }
}
