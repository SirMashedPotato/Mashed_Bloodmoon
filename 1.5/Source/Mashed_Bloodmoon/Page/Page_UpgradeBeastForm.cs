using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Page_UpgradeBeastForm : LycanthropePage
    {
        private readonly List<LycanthropeAbilityDef> AbilityList;
        private readonly List<LycanthropeTotemDef> TotemList;
        private static Vector2 scrollPosition = Vector2.zero;
        private static LycanthropeUpgradeType curTab = LycanthropeUpgradeType.Ability;
        private readonly List<TabRecord> tabs = new List<TabRecord>();
        private readonly List<UpgradeAmount> upgradeAmountList = new List<UpgradeAmount>();

        public float innerRectHeightTotem;
        public float innerRectHeightAbility;
        public static float rowHeight = Text.LineHeight * 6f;

        public override string PageTitle => "Mashed_Bloodmoon_UpgradeBeastForm".Translate().CapitalizeFirst() + ": " + pawn.NameShortColored;

        public enum LycanthropeUpgradeType
        {
            Ability,
            Claw,
            Totem
        }

        public Page_UpgradeBeastForm(HediffComp_Lycanthrope comp) : base(comp)
        {
            AbilityList = DefDatabase<LycanthropeAbilityDef>.AllDefsListForReading;
            TotemList = DefDatabase<LycanthropeTotemDef>.AllDefsListForReading.Where(x => x.displayAsTotem).ToList();
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
                curTab = LycanthropeUpgradeType.Ability;
            }, () => curTab == LycanthropeUpgradeType.Ability));
            /*
            tabs.Add(new TabRecord("Mashed_Bloodmoon_UpgradeTab_Claws".Translate(), delegate
            {
                curTab = LycanthropeUpgradeType.Claw;
            }, () => curTab == LycanthropeUpgradeType.Claw));
            */
            tabs.Add(new TabRecord("Mashed_Bloodmoon_UpgradeTab_Totems".Translate(), delegate
            {
                curTab = LycanthropeUpgradeType.Totem;
            }, () => curTab == LycanthropeUpgradeType.Totem));
        }

        private class UpgradeAmount
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
            Widgets.ButtonImage(new Rect(inRect.width - 30f, 0f, 30f, 30f), TexButton.Info, false, "Mashed_Bloodmoon_UpgradeBeastFormDesc".Translate());
            Widgets.ButtonImage(new Rect(inRect.width - (60f + rectPadding), 0f, 30f, 30f), TexButton.CategorizedResourceReadout, false, EffectsTooltip());

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
            mainRect = mainRect.ContractedBy(rectPadding);

            switch (curTab)
            {
                case LycanthropeUpgradeType.Ability:
                    DoUpgradeGrid(mainRect, AbilityList.Count());
                    break;

                case LycanthropeUpgradeType.Claw:
                    DoUpgradeGrid(mainRect, 2);
                    break;

                case LycanthropeUpgradeType.Totem:
                    DoUpgradeGrid(mainRect, TotemList.Count());
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

                cheatRectLeft1 = cheatRectLeft1.ContractedBy(rectPadding);
                if (Widgets.ButtonText(cheatRectLeft1, "Dev: min hearts", true))
                {
                    compLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts] = 0;
                }

                cheatRectLeft2 = cheatRectLeft2.ContractedBy(rectPadding);
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

                cheatRectRight1 = cheatRectRight1.ContractedBy(rectPadding);
                if (Widgets.ButtonText(cheatRectRight1, "Dev: +1 heart", true))
                {
                    if (consumedCount < LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.maxLevel)
                    {
                        compLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts]++;
                    }
                }

                cheatRectRight2 = cheatRectRight2.ContractedBy(rectPadding);
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
                tooltip += usedTotem.Key.StatBonusList(compLycanthrope, true);
            }

            tooltip += "Mashed_Bloodmoon_UnlockedAbilities".Translate();

            foreach (KeyValuePair<LycanthropeAbilityDef, int> unlockedAbility in compLycanthrope.unlockedAbilityTracker)
            {
                tooltip += "\n - " + unlockedAbility.Key.LabelCap;
            }

            return tooltip;
        }

        public void DoUpgradeGrid(Rect inRect, int listCount)
        {
            Rect scrollRect = inRect;
            Rect innerRect = scrollRect;
            innerRect.width -= 30f;

            float cellWidth = (innerRect.width / 2) - (rectPadding / 3f);
            float cellHeight = rowHeight;
            float rowCount = ((float)listCount / 2);
            if (rowCount % 1 != 0)
            {
                rowCount += 0.5f;
            }
            innerRect.height = Mathf.Round(rowCount) * (cellHeight + rectPadding);

            Widgets.BeginScrollView(scrollRect, ref scrollPosition, innerRect);
            int row = 0;
            int column = 0;
            Rect upgradeRect = new Rect(innerRect.x, innerRect.y, cellWidth, cellHeight);

            for(int i = 0; i < listCount; i++)
            {
                DoUpgradeCelll(upgradeRect, i);
                if (++column >= 2)
                {
                    upgradeRect.y += ((rectPadding / 2f) + cellHeight);
                    upgradeRect.x = innerRect.x;
                    column = 0;
                    row++;
                }
                else
                {
                    upgradeRect.x += ((rectPadding / 2f) + cellWidth);
                }
            }
        }

        public void DoUpgradeCelll(Rect inRect, int index)
        {
            Rect rightRect = inRect;
            Rect leftRect = inRect;
            rightRect.width = rightRect.height;
            leftRect.width -= rightRect.width + (rectPadding / 2f);
            rightRect.x += leftRect.width + (rectPadding / 2f);
            switch (curTab)
            {
                case LycanthropeUpgradeType.Ability:
                    DoAbilityCell(leftRect, rightRect, index);
                    break;

                case LycanthropeUpgradeType.Claw:

                    break;

                case LycanthropeUpgradeType.Totem:
                    DoTotemCell(leftRect, rightRect, index);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoAbilityCell(Rect leftRect, Rect rightRect, int index)
        {
            LycanthropeAbilityDef abilityDef = AbilityList[index];
            DoAbilityLeftRect(leftRect, abilityDef);
            DoAbilityRightRect(rightRect, abilityDef);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoAbilityLeftRect(Rect inRect, LycanthropeAbilityDef abilityDef)
        {
            AcceptanceReport acceptanceReport = abilityDef.PawnRequirementsMet(pawn);

            Widgets.DrawBoxSolidWithOutline(inRect, Widgets.WindowBGFillColor, Color.grey, 1);
            Rect mainRect = inRect.ContractedBy(rectPadding);
            RectDivider rectDivider = new RectDivider(mainRect, mainRect.GetHashCode(), null);

            RectDivider labelRect = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            Widgets.Label(labelRect.NewCol(abilityDef.LabelCap.GetWidthCached(), HorizontalJustification.Left), abilityDef.LabelCap);
            if (abilityDef.HasAbility(compLycanthrope))
            {
                TaggedString levelLabel = "Mashed_Bloodmoon_Unlocked".Translate();
                Widgets.Label(labelRect.NewCol(levelLabel.GetWidthCached(), HorizontalJustification.Right), levelLabel);
            }
            else if (!acceptanceReport)
            {
                TaggedString levelLabel = "Locked".Translate();
                Widgets.Label(labelRect.NewCol(levelLabel.GetWidthCached(), HorizontalJustification.Right), levelLabel);
            }

            var font = Text.Font;
            Text.Font = GameFont.Tiny;
            RectDivider descRect = rectDivider.NewRow(Text.LineHeight * 3f, VerticalJustification.Top);
            Widgets.Label(descRect, abilityDef.description);

            CompProperties_AbilityStressCost compStressCost = (CompProperties_AbilityStressCost)abilityDef.abilityDef.comps.Find(x => x is CompProperties_AbilityStressCost);
            if (compStressCost != null)
            {
                Rect stressCostRect = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
                Widgets.Label(stressCostRect, "Mashed_Bloodmoon_AbilityStressCost".Translate(compStressCost.stressCost));
            }
            else
            {
                CompProperties_AbilityHeartCost compHeartCost = (CompProperties_AbilityHeartCost)abilityDef.abilityDef.comps.Find(x => x is CompProperties_AbilityHeartCost);
                if (compHeartCost != null)
                {
                    Rect heartCostRect = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
                    Widgets.Label(heartCostRect, "Mashed_Bloodmoon_AbilityHeartCost".Translate(compHeartCost.heartCost));
                }
            }

            Text.Font = font;

            if (!abilityDef.HasAbility(compLycanthrope))
            {
                if (!acceptanceReport)
                {
                    Rect descriptionRect = mainRect;
                    descriptionRect.height = Text.LineHeight;
                    descriptionRect.width = descriptionRect.height;
                    descriptionRect.y += mainRect.height - descriptionRect.height;
                    descriptionRect.x += mainRect.width - descriptionRect.width;
                    Widgets.ButtonImage(descriptionRect, TexButton.Info, true, acceptanceReport.Reason.CapitalizeFirst());
                    return;
                }

                if (abilityDef.purchaseHeartCost > 0)
                {
                    Rect upgradeRect = mainRect;
                    upgradeRect.height = Text.LineHeight * 1.5f;
                    upgradeRect.width = 130f;
                    upgradeRect.y = inRect.y + inRect.height - upgradeRect.height - rectPadding;
                    upgradeRect.x = inRect.x + inRect.width - upgradeRect.width - rectPadding;
                    bool canPurchase = abilityDef.CanPurchase(compLycanthrope);
                    string unlockLabel = "Mashed_Bloodmoon_UnlockLabel".Translate(compLycanthrope.usedTotemTracker.TryGetValue(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0), abilityDef.purchaseHeartCost); ;
                    if (Widgets.ButtonText(upgradeRect, unlockLabel, true, canPurchase, active: canPurchase))
                    {
                        abilityDef.PurchaseAbility(compLycanthrope);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoAbilityRightRect(Rect inRect, LycanthropeAbilityDef abilityDef)
        {
            Widgets.DrawBoxSolidWithOutline(inRect, Widgets.WindowBGFillColor, Color.grey, 1);
            Rect mainRect = inRect.ContractedBy(rectPadding);
            GUI.DrawTexture(mainRect, ContentFinder<Texture2D>.Get(abilityDef.backgroundTexPath));
            Rect iconRect = mainRect.ContractedBy(rectPadding / 2f);
            GUI.DrawTexture(iconRect, abilityDef.abilityDef.uiIcon);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoTotemCell(Rect leftRect, Rect rightRect, int index)
        {
            LycanthropeTotemDef totemDef = TotemList[index];
            DoTotemLeftRect(leftRect, totemDef);
            DoTotemRightRect(rightRect, totemDef);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoTotemLeftRect(Rect inRect, LycanthropeTotemDef totemDef)
        {
            Widgets.DrawBoxSolidWithOutline(inRect, Widgets.WindowBGFillColor, Color.grey, 1);
            Rect mainRect = inRect.ContractedBy(rectPadding);
            RectDivider rectDivider = new RectDivider(mainRect, mainRect.GetHashCode(), null);

            RectDivider labelRect = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            Widgets.Label(labelRect.NewCol(totemDef.label.GetWidthCached(), HorizontalJustification.Left), totemDef.label);
            TaggedString levelLabel = "(" + compLycanthrope.usedTotemTracker.TryGetValue(totemDef, 0) + " / " + totemDef.maxLevel + ")";
            Widgets.Label(labelRect.NewCol(levelLabel.GetWidthCached(), HorizontalJustification.Right), levelLabel);

            var font = Text.Font;
            Text.Font = GameFont.Tiny;
            foreach (StatDef statDef in totemDef.statDefs)
            {
                RectDivider statRect = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
                TaggedString statLabel = totemDef.StatBonusLine(statDef, compLycanthrope, true);
                Widgets.Label(statRect.NewCol(statLabel.GetWidthCached(), HorizontalJustification.Left), statLabel);
            }
            Text.Font = font;

            if (totemDef.purchaseHeartCost > 0 && (!compLycanthrope.usedTotemTracker.ContainsKey(totemDef) || compLycanthrope.usedTotemTracker[totemDef] < totemDef.maxLevel))
            {
                Rect upgradeRect = mainRect;
                upgradeRect.height = Text.LineHeight * 1.5f;
                upgradeRect.y = inRect.y + inRect.height - upgradeRect.height - rectPadding;

                upgradeRect.SplitVerticallyWithMargin(out Rect upgradeSliderRect, out Rect upgradeButtonRect, rectPadding);

                UpgradeAmount upgradeAmount = upgradeAmountList.Where(x => x.totemTypeDef == totemDef).First();

                Widgets.HorizontalSlider(upgradeSliderRect, ref upgradeAmount.amount, new FloatRange(1, totemDef.MaxPurchaseableUpgrades(compLycanthrope)), "+" + upgradeAmount.amount, roundTo: 1f);

                bool canPurchase = totemDef.CanPurchase(compLycanthrope);
                string upgradeLabel = "Mashed_Bloodmoon_UpgradeLabel".Translate(compLycanthrope.usedTotemTracker.TryGetValue(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0), totemDef.purchaseHeartCost * upgradeAmount.amount);
                if (Widgets.ButtonText(upgradeButtonRect, upgradeLabel, true, canPurchase, active: canPurchase))
                {
                    totemDef.PurchaseTotemLevel(compLycanthrope, (int)upgradeAmount.amount);
                    foreach(UpgradeAmount val in upgradeAmountList)
                    {
                        val.amount = 1;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoTotemRightRect(Rect inRect, LycanthropeTotemDef totemDef)
        {
            Widgets.DrawBoxSolidWithOutline(inRect, Widgets.WindowBGFillColor, Color.grey, 1);
            Rect mainRect = inRect.ContractedBy(rectPadding);
            GUI.DrawTexture(mainRect, ContentFinder<Texture2D>.Get(totemDef.backgroundTexPath));
            Rect iconRect = mainRect.ContractedBy(rectPadding / 2f);
            GUI.DrawTexture(iconRect, ContentFinder<Texture2D>.Get(totemDef.IconTexPath), ScaleMode.StretchToFill, true, 0, totemDef.IconColor, 0f, 0f);

            if (totemDef.description != null)
            {
                Rect descriptionRect = iconRect;
                descriptionRect.height = Text.LineHeight;
                descriptionRect.width = descriptionRect.height;
                descriptionRect.y += iconRect.height - descriptionRect.height;
                descriptionRect.x += iconRect.width - descriptionRect.width;
                if (Widgets.ButtonImage(descriptionRect, TexButton.Info, true, totemDef.description))
                {
                    if (totemDef.totemThingDef != null)
                    {
                        Find.WindowStack.Add(new Dialog_InfoCard(totemDef.totemThingDef));
                    }
                }
            }
        }
    }
}
