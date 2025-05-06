using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Page_BeastHuntProgress : LycanthropePage
    {
        public const int columnNumber = 5;
        private readonly List<LycanthropeBeastHuntDef> BeastHuntListHeart;
        private readonly List<LycanthropeBeastHuntDef> BeastHuntListKill;
        private readonly List<LycanthropeBeastHuntDef> BeastHuntListOther;
        private static Vector2 scrollPosition = Vector2.zero;
        private static BeastHuntType curTab = BeastHuntType.Heart;
        private readonly List<TabRecord> tabs = new List<TabRecord>();

        public override string PageTitle => "Mashed_Bloodmoon_BeastHuntProgress".Translate().CapitalizeFirst() + ": " + pawn.NameShortColored;

        public Page_BeastHuntProgress(HediffComp_Lycanthrope comp) : base(comp)
        {
            BeastHuntListHeart = DefDatabase<LycanthropeBeastHuntDef>.AllDefsListForReading.Where(x => x.beastHuntType == BeastHuntType.Heart).ToList();
            BeastHuntListKill = DefDatabase<LycanthropeBeastHuntDef>.AllDefsListForReading.Where(x => x.beastHuntType == BeastHuntType.Kill).ToList();
            BeastHuntListOther = DefDatabase<LycanthropeBeastHuntDef>.AllDefsListForReading.Where(x => x.beastHuntType == BeastHuntType.Other).ToList();
            ReadySettingsTabs();
        }

        private void ReadySettingsTabs()
        {
            tabs.Add(new TabRecord("Mashed_Bloodmoon_BeastHuntTab_Hearts".Translate(), delegate
            {
                curTab = BeastHuntType.Heart;
            }, () => curTab == BeastHuntType.Heart));

            tabs.Add(new TabRecord("Mashed_Bloodmoon_BeastHuntTab_Kills".Translate(), delegate
            {
                curTab = BeastHuntType.Kill;
            }, () => curTab == BeastHuntType.Kill));

            if (!BeastHuntListOther.NullOrEmpty())
            {
                tabs.Add(new TabRecord("Mashed_Bloodmoon_BeastHuntTab_Other".Translate(), delegate
                {
                    curTab = BeastHuntType.Other;
                }, () => curTab == BeastHuntType.Other));
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            DrawPageTitle(inRect);
            Widgets.ButtonImage(new Rect(inRect.width - 30f, 0f, 30f, 30f), TexButton.Info, false, "Mashed_Bloodmoon_BeastHuntDesc".Translate(pawn, compLycanthrope.completedBeastHunts));

            if (DebugSettings.ShowDevGizmos)
            {
                Rect devRecalculate = new Rect(inRect.width - 220f, 0f, 180f, 30f);
                if (Widgets.ButtonText(devRecalculate, "Dev: recalculate thought", true))
                {
                    RecalculateBeastHuntTHought();
                }
            }

            Rect mainRect = new Rect(inRect.x, inRect.yMin + 45f, inRect.width, inRect.height - 45f);
            TabDrawer.DrawTabs(new Rect(mainRect.x, mainRect.yMin + 45f, mainRect.width, mainRect.height - 45f), tabs);

            mainRect.yMin += rectLimitY;
            DoBottomButtons(mainRect, showNext: false);
            mainRect.height -= rectLimitY;
            DoBeastHuntWindow(mainRect, curTab == BeastHuntType.Heart ? BeastHuntListHeart : curTab == BeastHuntType.Kill ? BeastHuntListKill : BeastHuntListOther);
            Widgets.EndScrollView();
        }

        private void RecalculateBeastHuntTHought()
        {
            compLycanthrope.completedBeastHunts = compLycanthrope.beastHuntTracker.Where(x => x.Key.Completed(x.Value)).Count();
        }

        public void DoBeastHuntWindow(Rect inRect, List<LycanthropeBeastHuntDef> beastHuntList)
        {
            Rect scrollRect = inRect;
            Rect innerRect = scrollRect;
            innerRect.width -= 30f;

            float gridWidth = (innerRect.width / columnNumber) - (rectPadding / 2f);
            float gridHeight = gridWidth * 1.6f;
            float rowCount = ((float)beastHuntList.Count / columnNumber);
            if (rowCount % 1 != 0)
            {
                rowCount += 0.5f;
            }
            innerRect.height = Mathf.Round(rowCount) * (gridHeight + rectPadding);

            Widgets.BeginScrollView(scrollRect, ref scrollPosition, innerRect);
            int row = 0;
            int column = 0;
            Rect beastHuntRect = new Rect(innerRect.x, innerRect.y, gridWidth, gridHeight);

            foreach (LycanthropeBeastHuntDef beastHuntDef in beastHuntList)
            {
                DoBeastHuntGrid(beastHuntRect, beastHuntDef);
                if (++column >= columnNumber)
                {
                    beastHuntRect.y += ((rectPadding / 2f) + gridHeight);
                    beastHuntRect.x = innerRect.x;
                    column = 0;
                    row++;
                }
                else
                {
                    beastHuntRect.x += ((rectPadding / 2f) + gridWidth);
                }
            }
        }

        public void DoBeastHuntGrid(Rect inRect, LycanthropeBeastHuntDef greatBeastDef)
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

        public void DoGridUpperRect(Rect inRect, LycanthropeBeastHuntDef greatBeastDef)
        {
            Rect mainRect = inRect.ContractedBy(rectPadding);
            GUI.DrawTexture(mainRect, ContentFinder<Texture2D>.Get(greatBeastDef.backgroundTexPath));
            GUI.DrawTexture(mainRect, ContentFinder<Texture2D>.Get(greatBeastDef.heartTexPath));
            if (greatBeastDef.Completed(compLycanthrope))
            {
                GUI.DrawTexture(mainRect, ContentFinder<Texture2D>.Get(greatBeastDef.completedTexPath));
            }
            else
            {
                GUI.DrawTexture(mainRect, ContentFinder<Texture2D>.Get(greatBeastDef.heartTexPath), ScaleMode.StretchToFill, true, 0f, color: new Color(0,0,0,0.8f), 0f, 0f);
                if (greatBeastDef.targetCount > 1)
                {
                    TaggedString label = greatBeastDef.Progress(compLycanthrope) + "/" + greatBeastDef.targetCount;
                    Rect progressRect = mainRect.ContractedBy(rectPadding);
                    progressRect.height = Text.LineHeight;
                    progressRect.y += mainRect.height - (progressRect.height + (rectPadding * 1.5f));
                    Widgets.Label(progressRect, label);
                }
            }
            if (greatBeastDef.extraTooltip !=  null)
            {
                Rect extraTooltipRect = mainRect.ContractedBy(rectPadding);
                extraTooltipRect.height = Text.LineHeight;
                extraTooltipRect.width = extraTooltipRect.height;
                extraTooltipRect.y += mainRect.height - (extraTooltipRect.height + (rectPadding * 1.5f));
                extraTooltipRect.x += mainRect.width - extraTooltipRect.height - (rectPadding * 1.5f);
                Widgets.ButtonImage(extraTooltipRect, TexButton.Info, false, greatBeastDef.extraTooltip);
            }
        }

        public void DoGridLowerRect(Rect inRect, LycanthropeBeastHuntDef beastHuntDef)
        {
            Rect detailsRect = inRect.ContractedBy(rectPadding);
            TaggedString label = beastHuntDef.LabelCap;
            detailsRect.SplitHorizontally(detailsRect.height / 3f, out Rect upperRect, out Rect lowerRect);
            var anchor = Text.Anchor;
            var font = Text.Font;
            Text.Anchor = TextAnchor.MiddleCenter;
            if (beastHuntDef.IsHidden(compLycanthrope))
            {
                Widgets.LabelFit(upperRect, "Mashed_Bloodmoon_BeastHuntHidden".Translate());
                Text.Font = GameFont.Tiny;
                Widgets.Label(lowerRect, "Mashed_Bloodmoon_BeastHuntHiddenDesc".Translate());
            }
            else if (beastHuntDef.AnomalyIsHidden())
            {
                Widgets.LabelFit(upperRect, "Mashed_Bloodmoon_BeastHuntHidden".Translate());
                Text.Font = GameFont.Tiny;
                Widgets.Label(lowerRect, "Mashed_Bloodmoon_BeastHuntHiddenAnomalyDesc".Translate(beastHuntDef.anomalyLevelToReveal));
            }
            else
            {
                Widgets.LabelFit(upperRect, label);
                Text.Font = GameFont.Tiny;
                Widgets.Label(lowerRect, beastHuntDef.description);
            }
            
            Text.Anchor = anchor;
            Text.Font = font;
        }
    }
}
