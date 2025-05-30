﻿using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Page_UpgradeBeastForm : LycanthropePage
    {
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

        public override string PageTitle => "Mashed_Bloodmoon_UpgradeBeastForm".Translate().CapitalizeFirst() + ": " + pawn.NameShortColored;

        private readonly List<LycanthropeTotemDef> TotemList;
        private readonly List<LycanthropeAbilityDef> AbilityList;
        private static Vector2 scrollPositionTotem = Vector2.zero;
        private static Vector2 scrollPositionAbility = Vector2.zero;
        private readonly List<UpgradeAmount> upgradeAmountList = new List<UpgradeAmount>();

        public float innerRectHeightTotem;
        public float innerRectHeightAbility;
        public static float rowHeight = Text.LineHeight * 6f;

        public Page_UpgradeBeastForm(HediffComp_Lycanthrope comp) : base(comp)
        {
            TotemList = DefDatabase<LycanthropeTotemDef>.AllDefsListForReading.Where(x => x.displayAsTotem).ToList();
            AbilityList = DefDatabase<LycanthropeAbilityDef>.AllDefsListForReading;
            innerRectHeightTotem = TotemList.Count * (rowHeight + (rectPadding / 2f));
            innerRectHeightAbility = AbilityList.Count * (rowHeight + (rectPadding / 2f));

            foreach (LycanthropeTotemDef def in TotemList)
            {
                if (def.purchaseHeartCost > 0)
                {
                    upgradeAmountList.Add(new UpgradeAmount(def));
                }
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            DrawPageTitle(inRect);
            Widgets.ButtonImage(new Rect(inRect.width - 30f, 0f, 30f, 30f), TexButton.Info, false, "Mashed_Bloodmoon_UpgradeBeastFormDesc".Translate());
            Widgets.ButtonImage(new Rect(inRect.width - (60f + rectPadding), 0f, 30f, 30f), TexButton.CategorizedResourceReadout, false, EffectsTooltip());

            inRect.yMin += rectLimitY;
            DoBottomButtons(inRect, showNext: false);

            Rect consumedHeartsRect = inRect;
            consumedHeartsRect.height = Text.LineHeight * 2f;
            DoConsumedHeartsBar(consumedHeartsRect);

            Rect leftRect = inRect;
            leftRect.width = (inRect.width / 2) - (rectPadding / 2);
            leftRect.y += consumedHeartsRect.height + rectPadding;
            leftRect.height -= (consumedHeartsRect.height + rectLimitY + rectPadding);
            DoLeftSide(leftRect);

            Rect rightRect = leftRect;
            rightRect.x += rightRect.width + (rectPadding);
            DoRightSide(rightRect);
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

        public void DoLeftSide(Rect inRect)
        {
            Rect scrollRect = inRect;

            Rect innerRect = scrollRect;
            innerRect.height = innerRectHeightTotem;
            innerRect.width -= 30f;

            Widgets.BeginScrollView(scrollRect, ref scrollPositionTotem, innerRect);
            int index = 0;
            foreach (LycanthropeTotemDef totemDef in TotemList)
            {
                Rect totemRect = innerRect;
                totemRect.height = rowHeight;
                totemRect.y += ((rectPadding / 2f) + rowHeight) * index;
                DoTotemRow(totemRect, totemDef);
                index++;
            }
            Widgets.EndScrollView();
        }

        public void DoTotemRow(Rect inRect, LycanthropeTotemDef totemDef)
        {
            Rect rightRect = inRect;
            Rect leftRect = inRect;
            rightRect.width = rightRect.height;
            leftRect.width -= rightRect.width + (rectPadding / 2f);
            rightRect.x += leftRect.width + (rectPadding / 2f);

            DoTotemLeftRect(leftRect, totemDef);
            DoTotemRightRect(rightRect, totemDef);
        }

        public void DoTotemLeftRect(Rect inRect, LycanthropeTotemDef totemDef)
        {
            Widgets.DrawMenuSection(inRect);
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

        public void DoTotemRightRect(Rect inRect, LycanthropeTotemDef totemDef)
        {
            Widgets.DrawMenuSection(inRect);
            Rect mainRect = inRect.ContractedBy(rectPadding);
            GUI.DrawTexture(mainRect, ContentFinder<Texture2D>.Get(totemDef.IconTexPath), ScaleMode.StretchToFill, true, 0, totemDef.IconColor, 0f, 0f);

            if (totemDef.description != null)
            {
                Rect descriptionRect = mainRect;
                descriptionRect.height = Text.LineHeight;
                descriptionRect.width = descriptionRect.height;
                descriptionRect.y += mainRect.height - descriptionRect.height;
                descriptionRect.x += mainRect.width - descriptionRect.width;
                if (Widgets.ButtonImage(descriptionRect, TexButton.Info, true, totemDef.description))
                {
                    if (totemDef.totemThingDef != null)
                    {
                        Find.WindowStack.Add(new Dialog_InfoCard(totemDef.totemThingDef));
                    }
                }
            }
        }

        public void DoRightSide(Rect inRect)
        {
            Rect scrollRect = inRect;

            Rect innerRect = scrollRect;
            innerRect.height = innerRectHeightAbility;
            innerRect.width -= 30f;

            Widgets.BeginScrollView(scrollRect, ref scrollPositionAbility, innerRect);
            int index = 0;
            foreach (LycanthropeAbilityDef abilityDef in AbilityList)
            {
                Rect abilityRect = innerRect;
                abilityRect.height = rowHeight;
                abilityRect.y += ((rectPadding / 2f) + rowHeight) * index;
                DoAbilityRow(abilityRect, abilityDef);
                index++;
            }
            Widgets.EndScrollView();
        }

        public void DoAbilityRow(Rect inRect, LycanthropeAbilityDef abilityDef)
        {
            Rect rightRect = inRect;
            Rect leftRect = inRect;
            rightRect.width = rightRect.height;
            leftRect.width -= rightRect.width + (rectPadding / 2f);
            rightRect.x += leftRect.width + (rectPadding / 2f);

            DoAbilityLeftRect(leftRect, abilityDef);
            DoAbilityRightRect(rightRect, abilityDef);
        }

        public void DoAbilityLeftRect(Rect inRect, LycanthropeAbilityDef abilityDef)
        {
            AcceptanceReport acceptanceReport = abilityDef.PawnRequirementsMet(pawn);

            Widgets.DrawMenuSection(inRect);
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

        public void DoAbilityRightRect(Rect inRect, LycanthropeAbilityDef abilityDef)
        {
            Widgets.DrawMenuSection(inRect);
            Rect mainRect = inRect.ContractedBy(rectPadding);
            GUI.DrawTexture(mainRect, abilityDef.abilityDef.uiIcon);
        }
    }
}
