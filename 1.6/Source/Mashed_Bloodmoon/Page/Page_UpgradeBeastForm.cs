using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Page_UpgradeBeastForm : LycanthropePage
    {
        public const int columnCount = 2;
        public const int altColumnCount = 3;
        private readonly List<LycanthropeAbilityDef> AbilityList;
        private readonly List<LycanthropeClawTypeDef> ClawList;
        private readonly List<LycanthropeTotemDef> TotemList; 
        private readonly List<LycanthropeTraitDef> TraitList;
        private static Vector2 scrollPosition = Vector2.zero;
        private static UpgradeType curTab = UpgradeType.Ability;
        private readonly List<TabRecord> tabs = new List<TabRecord>();
        private List<UpgradeAmount> upgradeAmountList = new List<UpgradeAmount>();

        public float innerRectHeightTotem;
        public float innerRectHeightAbility;
        public static float rowHeight = Text.LineHeight * 6f;

        public override string PageTitle => "Mashed_Bloodmoon_UpgradeBeastForm".Translate().CapitalizeFirst() + ": " + pawn.NameShortColored;

        public enum UpgradeType
        {
            Ability,
            Claw,
            Totem,
            Trait
        }

        public Page_UpgradeBeastForm(HediffComp_Lycanthrope comp) : base(comp)
        {
            AbilityList = DefDatabase<LycanthropeAbilityDef>.AllDefsListForReading;
            ClawList = DefDatabase<LycanthropeClawTypeDef>.AllDefsListForReading;
            TotemList = DefDatabase<LycanthropeTotemDef>.AllDefsListForReading.Where(x => x.displayAsTotem).ToList();
            TraitList = DefDatabase<LycanthropeTraitDef>.AllDefsListForReading.OrderBy(x=>x.traitDef.DataAtDegree(x.traitDegree).GetLabelCapFor(pawn)).ToList();
            ReadySettingsTabs();

            foreach (LycanthropeTotemDef def in TotemList)
            {
                if (def.purchaseHeartCost > 0)
                {
                    upgradeAmountList.Add(new UpgradeAmount(def));
                }
            }
        }

        private void ReadySettingsTabs()
        {
            tabs.Add(new TabRecord("Mashed_Bloodmoon_UpgradeTab_Abilities".Translate(), delegate
            {
                curTab = UpgradeType.Ability;
            }, () => curTab == UpgradeType.Ability));

            tabs.Add(new TabRecord("Mashed_Bloodmoon_UpgradeTab_Claws".Translate(), delegate
            {
                curTab = UpgradeType.Claw;
            }, () => curTab == UpgradeType.Claw));

            tabs.Add(new TabRecord("Mashed_Bloodmoon_UpgradeTab_Totems".Translate(), delegate
            {
                curTab = UpgradeType.Totem;
            }, () => curTab == UpgradeType.Totem));

            tabs.Add(new TabRecord("Traits".Translate(), delegate
            {
                curTab = UpgradeType.Trait;
            }, () => curTab == UpgradeType.Trait));
        }

        public class UpgradeAmount
        {
            public LycanthropeTotemDef totemTypeDef;
            public float amount;

            public UpgradeAmount(LycanthropeTotemDef def)
            {
                totemTypeDef = def;
                amount = 1f;
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            DrawPageTitle(inRect);
            Widgets.ButtonImage(new Rect(inRect.width - 30f, 0f, 30f, 30f), TexButton.Info, false, "Mashed_Bloodmoon_UpgradeBeastFormDesc".Translate().Resolve());
            Widgets.ButtonImage(new Rect(inRect.width - (60f + Assets.RectPadding), 0f, 30f, 30f), TexButton.CategorizedResourceReadout, false, EffectsTooltip());

            inRect.yMin += rectLimitY;
            Rect consumedHeartsRect = inRect;
            consumedHeartsRect.height = Text.LineHeight * 2f;
            DoConsumedHeartsBar(consumedHeartsRect);

            Rect mainRect = new Rect(inRect.x, inRect.yMin + 45f, inRect.width, inRect.height - 45f);
            TabDrawer.DrawTabs(new Rect(mainRect.x, mainRect.yMin + 45f, mainRect.width, mainRect.height - 45f), tabs);

            mainRect.yMin += rectLimitY;
            DoBottomButtons(mainRect, showNext: false);
            mainRect.height -= rectLimitY;
            Widgets.DrawMenuSection(mainRect);
            mainRect = mainRect.ContractedBy(Assets.RectPadding);

            switch (curTab)
            {
                case UpgradeType.Ability:
                    DoUpgradeGrid(mainRect, AbilityList.Count());
                    break;

                case UpgradeType.Claw:
                    DoUpgradeGrid(mainRect, ClawList.Count());
                    break;

                case UpgradeType.Totem:
                    DoUpgradeGrid(mainRect, TotemList.Count());
                    break;

                case UpgradeType.Trait:
                    DoUpgradeGrid(mainRect, TraitList.Count());
                    break;
            }

            Widgets.EndScrollView();
        }

        public void DoConsumedHeartsBar(Rect inRect)
        {
            int consumedCount = compLycanthrope.usedTotemTracker.TryGetValue(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0);
            int maxLevel = LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.maxLevel;
            float fillPercent = (float)consumedCount / maxLevel;
            Widgets.FillableBar(inRect, fillPercent, Assets.ConsumedHeartsFillTex, Texture2D.grayTexture, true);
            var anchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(inRect, LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.label + " (" + consumedCount + " / " + maxLevel + ")");
            Text.Anchor = anchor;

            if (DebugSettings.ShowDevGizmos)
            {
                //Left
                Rect devCheatRectLeft = inRect;
                devCheatRectLeft.width = 320f;

                devCheatRectLeft.SplitVertically(devCheatRectLeft.width / 2f, out Rect cheatRectLeft1, out Rect cheatRectLeft2);

                cheatRectLeft1 = cheatRectLeft1.ContractedBy(Assets.RectPadding);
                if (Widgets.ButtonText(cheatRectLeft1, "Dev: min hearts", true))
                {
                    compLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts] = 0;
                }

                cheatRectLeft2 = cheatRectLeft2.ContractedBy(Assets.RectPadding);
                if (Widgets.ButtonText(cheatRectLeft2, "Dev: -1 heart", true))
                {
                    if (consumedCount > 0)
                    {
                        compLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts]--;
                    }
                }

                //Right
                Rect devCheatRectRight = devCheatRectLeft;
                devCheatRectRight.x = inRect.width - devCheatRectRight.width;

                devCheatRectRight.SplitVertically(devCheatRectRight.width / 2f, out Rect cheatRectRight1, out Rect cheatRectRight2);

                cheatRectRight1 = cheatRectRight1.ContractedBy(Assets.RectPadding);
                if (Widgets.ButtonText(cheatRectRight1, "Dev: +1 heart", true))
                {
                    if (consumedCount < LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.maxLevel)
                    {
                        compLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts]++;
                    }
                }

                cheatRectRight2 = cheatRectRight2.ContractedBy(Assets.RectPadding);
                if (Widgets.ButtonText(cheatRectRight2, "Dev: max hearts", true))
                {
                    compLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts] = LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.maxLevel;
                }
            }
        }

        public string EffectsTooltip()
        {
            string tooltip = "Mashed_Bloodmoon_CurrentBonuses".Translate();
            foreach (KeyValuePair<LycanthropeTotemDef, int> usedTotem in compLycanthrope.usedTotemTracker)
            {
                tooltip += "\n - " + usedTotem.Key.StatBonusLine(compLycanthrope, true);
            }

            tooltip += "Mashed_Bloodmoon_UnlockedAbilities".Translate();
            foreach (LycanthropeAbilityDef unlockedAbility in compLycanthrope.unlockedAbilityTracker)
            {
                tooltip += "\n - " + unlockedAbility.LabelCap;
            }

            tooltip += "Mashed_Bloodmoon_UnlockedClaws".Translate();
            foreach (LycanthropeClawTypeDef unlockedClaw in compLycanthrope.unlockedClawTracker)
            {
                tooltip += "\n - " + unlockedClaw.LabelCap;
                if (compLycanthrope.equippedClawType == unlockedClaw)
                {
                    tooltip += " " + "Mashed_Bloodmoon_EquippedClaw".Translate();
                }
            }

            return tooltip;
        }

        public void DoUpgradeGrid(Rect inRect, int listCount)
        {
            Rect scrollRect = inRect;
            Rect innerRect = scrollRect;
            innerRect.width -= 30f;
            int finalColumnCount = curTab == UpgradeType .Claw || curTab == UpgradeType.Trait ? altColumnCount : columnCount;

            float cellWidth = (innerRect.width / finalColumnCount) - (Assets.RectPadding / 3f);
            float cellHeight = rowHeight;
            float rowCount = ((float)listCount / finalColumnCount);
            if (rowCount % 1 != 0)
            {
                rowCount += 0.5f;
            }
            innerRect.height = Mathf.Round(rowCount) * (cellHeight + Assets.RectPadding);

            Widgets.BeginScrollView(scrollRect, ref scrollPosition, innerRect);
            int row = 0;
            int column = 0;
            Rect upgradeRect = new Rect(innerRect.x, innerRect.y, cellWidth, cellHeight);

            for(int i = 0; i < listCount; i++)
            {
                DoUpgradeCelll(upgradeRect, i);
                if (++column >= finalColumnCount)
                {
                    upgradeRect.y += ((Assets.RectPadding / 2f) + cellHeight);
                    upgradeRect.x = innerRect.x;
                    column = 0;
                    row++;
                }
                else
                {
                    upgradeRect.x += ((Assets.RectPadding / 2f) + cellWidth);
                }
            }
        }

        public void DoUpgradeCelll(Rect inRect, int index)
        {
            Rect rightRect = inRect;
            Rect leftRect = inRect;
            rightRect.width = rightRect.height;
            leftRect.width -= rightRect.width + (Assets.RectPadding / 2f);
            rightRect.x += leftRect.width + (Assets.RectPadding / 2f);
            switch (curTab)
            {
                case UpgradeType.Ability:
                    Tab_UpgradeAbility.DoCell(leftRect, rightRect, AbilityList[index], compLycanthrope);
                    break;

                case UpgradeType.Claw:
                    Tab_UpgradeClaw.DoCell(inRect, ClawList[index], compLycanthrope);
                    break;

                case UpgradeType.Totem:
                    Tab_UpgradeTotem.DoCell(leftRect, rightRect, TotemList[index], compLycanthrope, ref upgradeAmountList);
                    break;

                case UpgradeType.Trait:
                    Tab_UpgradeTrait.DoCell(inRect, TraitList[index], compLycanthrope);
                    break;
            }
        }
    }
}
