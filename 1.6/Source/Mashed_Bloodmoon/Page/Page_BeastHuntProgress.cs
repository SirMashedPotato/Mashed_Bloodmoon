using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Page_BeastHuntProgress : LycanthropePage
    {
        public const int columnCount = 5;
        private readonly List<LycanthropeBeastHuntDef> BeastHuntListHeart;
        private readonly List<LycanthropeBeastHuntDef> BeastHuntListKill;
        private readonly List<LycanthropeBeastHuntDef> BeastHuntListProficiency;
        private readonly List<LycanthropeBeastHuntDef> BeastHuntListOther;
        private static Vector2 scrollPosition = Vector2.zero;
        private static BeastHuntType curTab = BeastHuntType.Heart;
        private readonly List<TabRecord> tabs = new List<TabRecord>();

        public override string PageTitle => "Mashed_Bloodmoon_BeastHuntProgress".Translate().CapitalizeFirst() + ": " + pawn.NameShortColored;

        public Page_BeastHuntProgress(HediffComp_Lycanthrope comp) : base(comp)
        {
            BeastHuntListHeart = DefDatabase<LycanthropeBeastHuntDef>.AllDefsListForReading.Where(x => x.beastHuntType == BeastHuntType.Heart).ToList();
            BeastHuntListKill = DefDatabase<LycanthropeBeastHuntDef>.AllDefsListForReading.Where(x => x.beastHuntType == BeastHuntType.Kill).ToList();
            BeastHuntListProficiency = DefDatabase<LycanthropeBeastHuntDef>.AllDefsListForReading.Where(x => x.beastHuntType == BeastHuntType.Proficiency).ToList();
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

            tabs.Add(new TabRecord("Mashed_Bloodmoon_BeastHuntTab_Proficiency".Translate(), delegate
            {
                curTab = BeastHuntType.Proficiency;
            }, () => curTab == BeastHuntType.Proficiency));

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
            Widgets.ButtonImage(new Rect(inRect.width - 30f, 0f, 30f, 30f), TexButton.Info, false, "Mashed_Bloodmoon_BeastHuntDesc".Translate(pawn, compLycanthrope.completedBeastHuntsCount));

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
            Widgets.DrawMenuSection(mainRect);
            mainRect = mainRect.ContractedBy(Assets.RectPadding);

            DoBeastHuntGrid(mainRect, curTab == BeastHuntType.Heart ? BeastHuntListHeart : curTab == BeastHuntType.Kill ? BeastHuntListKill 
                : curTab == BeastHuntType.Proficiency ? BeastHuntListProficiency : BeastHuntListOther);
            Widgets.EndScrollView();
        }

        private void RecalculateBeastHuntTHought()
        {
            compLycanthrope.completedBeastHuntsCount = compLycanthrope.beastHuntTracker.Where(x => x.Key.Completed(x.Value)).Count();
        }

        public void DoBeastHuntGrid(Rect inRect, List<LycanthropeBeastHuntDef> beastHuntList)
        {
            Rect scrollRect = inRect;
            Rect innerRect = scrollRect;
            innerRect.width -= 30f;

            int finalColumnCount = columnCount;

            if (!Text.TinyFontSupported && finalColumnCount > 1)
            {
                finalColumnCount -= 1;
            }

            float cellWidth = (innerRect.width / finalColumnCount) - (Assets.RectPadding / 3f);
            float cellHeight = cellWidth * 1.6f;
            float rowCount = ((float)beastHuntList.Count / finalColumnCount);
            if (rowCount % 1 != 0)
            {
                rowCount += 0.5f;
            }
            innerRect.height = Mathf.Round(rowCount) * (cellHeight + Assets.RectPadding);

            Widgets.BeginScrollView(scrollRect, ref scrollPosition, innerRect);
            int row = 0;
            int column = 0;
            Rect beastHuntRect = new Rect(innerRect.x, innerRect.y, cellWidth, cellHeight);

            foreach (LycanthropeBeastHuntDef beastHuntDef in beastHuntList)
            {
                DoBeastHuntCell(beastHuntRect, beastHuntDef);
                if (++column >= finalColumnCount)
                {
                    beastHuntRect.y += ((Assets.RectPadding / 2f) + cellHeight);
                    beastHuntRect.x = innerRect.x;
                    column = 0;
                    row++;
                }
                else
                {
                    beastHuntRect.x += ((Assets.RectPadding / 2f) + cellWidth);
                }
            }
        }

        public void DoBeastHuntCell(Rect inRect, LycanthropeBeastHuntDef beastHuntDef)
        {
            Widgets.DrawBoxSolidWithOutline(inRect, Widgets.WindowBGFillColor, Color.grey, 1);
            Rect upperRect = inRect;
            Rect lowerRect = inRect;
            upperRect.height = upperRect.width;
            lowerRect.height -= upperRect.height + (Assets.RectPadding / 2f);
            lowerRect.y += upperRect.height + (Assets.RectPadding / 2f);
            
            DoCellUpperRect(upperRect, beastHuntDef);
            DoCellGridLowerRect(lowerRect, beastHuntDef);
        }

        public void DoCellUpperRect(Rect inRect, LycanthropeBeastHuntDef beastHuntDef)
        {
            Rect mainRect = inRect.ContractedBy(Assets.RectPadding);
            GUI.DrawTexture(mainRect, ContentFinder<Texture2D>.Get(beastHuntDef.backgroundTexPath));
            Rect heartRect = mainRect.ContractedBy(Assets.RectPadding/2f);
            Texture2D heartTex = ContentFinder<Texture2D>.Get(beastHuntDef.heartTexPath);
            GUI.DrawTexture(heartRect, heartTex, ScaleMode.ScaleToFit);

            RectDivider rectDivider = new RectDivider(mainRect.ContractedBy(Assets.RectPadding / 2f), mainRect.GetHashCode(), null);
            RectDivider infoRect = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Bottom);

            if (beastHuntDef.Completed(compLycanthrope))
            {
                GUI.DrawTexture(heartRect, ContentFinder<Texture2D>.Get(beastHuntDef.completedTexPath));
            }

            PageUtility.ModSourceIcon(ref infoRect, beastHuntDef, HorizontalJustification.Left);

            if (!beastHuntDef.Completed(compLycanthrope))
            {
                GUI.DrawTexture(heartRect, heartTex, ScaleMode.ScaleToFit, true, 0f, color: new Color(0,0,0,0.8f), 0f, 0f);
                if (beastHuntDef.targetCount > 1)
                {
                    TaggedString label = beastHuntDef.Progress(compLycanthrope) + "/" + beastHuntDef.targetCount;
                    Widgets.Label(infoRect.NewCol(label.GetWidthCached(), HorizontalJustification.Left), label);
                }
            }

            if (DebugSettings.ShowDevGizmos)
            {
                RectDivider devRect = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
                if (!beastHuntDef.Completed(compLycanthrope))
                {
                    if (Widgets.ButtonImage(devRect.NewCol(devRect.Rect.height, HorizontalJustification.Right), TexButton.Plus, false, "Dev: instantly complete beast hunt"))
                    {
                        beastHuntDef.CompleteBeastHunt(compLycanthrope, pawn);
                    }
                }
                else
                {
                    if (Widgets.ButtonImage(devRect.NewCol(devRect.Rect.height, HorizontalJustification.Right), TexButton.Minus, false, "Dev: reset beast hunt"))
                    {
                        compLycanthrope.beastHuntTracker.SetOrAdd(beastHuntDef, 0);
                        compLycanthrope.completedBeastHuntsCount--;
                    }
                }
                
            }

            if (beastHuntDef.extraTooltip !=  null)
            {
                Widgets.ButtonImage(infoRect.NewCol(infoRect.Rect.height, HorizontalJustification.Right), TexButton.Info, false, beastHuntDef.extraTooltip);
            }

            //Last to ensure other stuff can be clicked if needed
            if (Widgets.ButtonInvisible(heartRect, false))
            {
                DoHeartClickAction(beastHuntDef);
            }
        }

        private void DoHeartClickAction(LycanthropeBeastHuntDef beastHuntDef)
        {
            if (beastHuntDef.IsHidden(compLycanthrope) || beastHuntDef.AnomalyIsHidden())
            {
                return;
            }

            Def targetDef = beastHuntDef.targetKindDef ?? (Def)beastHuntDef.targetThingDef ?? beastHuntDef.targetXenotypeDef;

            if (targetDef == null)
            {
                return;
            }

            Dialog_InfoCard dialog_InfoCard = new Dialog_InfoCard(targetDef);
            Find.WindowStack.Add(dialog_InfoCard);
        }

        public void DoCellGridLowerRect(Rect inRect, LycanthropeBeastHuntDef beastHuntDef)
        {
            Rect detailsRect = inRect.ContractedBy(Assets.RectPadding);
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
