using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public static class Tab_UpgradeTotem
    {
        public static void DoCell(Rect leftRect, Rect rightRect, LycanthropeTotemDef totemDef, HediffComp_Lycanthrope compLycanthrope, ref List<Page_UpgradeBeastForm.UpgradeAmount> upgradeAmountList)
        {
            DoLeftRect(leftRect, totemDef, compLycanthrope, ref upgradeAmountList);
            DoRightRect(rightRect, totemDef);
        }

        public static void DoLeftRect(Rect inRect, LycanthropeTotemDef totemDef, HediffComp_Lycanthrope compLycanthrope, ref List<Page_UpgradeBeastForm.UpgradeAmount> upgradeAmountList)
        {
            Widgets.DrawBoxSolidWithOutline(inRect, Widgets.WindowBGFillColor, Color.grey, 1);
            Rect mainRect = inRect.ContractedBy(Assets.RectPadding);
            RectDivider rectDivider = new RectDivider(mainRect, mainRect.GetHashCode(), null);

            RectDivider labelRect = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            Widgets.Label(labelRect.NewCol(totemDef.label.GetWidthCached(), HorizontalJustification.Left), totemDef.label);
            TaggedString levelLabel = "(" + compLycanthrope.usedTotemTracker.TryGetValue(totemDef, 0) + " / " + totemDef.maxLevel + ")";
            Widgets.Label(labelRect.NewCol(levelLabel.GetWidthCached(), HorizontalJustification.Right), levelLabel);

            var font = Text.Font;
            Text.Font = GameFont.Tiny;

            //stat summary
            RectDivider statRect1 = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            TaggedString statLabel1 = totemDef.StatBonusLine(compLycanthrope, true);
            Widgets.Label(statRect1, statLabel1);

            RectDivider statRect2 = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            TaggedString statLabel2 = totemDef.StatPerLevelLine();
            Widgets.Label(statRect2, statLabel2);

            Text.Font = font;

            if (totemDef.purchaseHeartCost > 0 && (!compLycanthrope.usedTotemTracker.ContainsKey(totemDef) || compLycanthrope.usedTotemTracker[totemDef] < totemDef.maxLevel))
            {
                Rect upgradeRect = mainRect;
                upgradeRect.height = Text.LineHeight * 1.5f;
                upgradeRect.y = inRect.y + inRect.height - upgradeRect.height - Assets.RectPadding;

                upgradeRect.SplitVerticallyWithMargin(out Rect upgradeSliderRect, out Rect upgradeButtonRect, Assets.RectPadding);

                Page_UpgradeBeastForm.UpgradeAmount upgradeAmount = upgradeAmountList.Where(x => x.totemTypeDef == totemDef).First();

                Widgets.HorizontalSlider(upgradeSliderRect, ref upgradeAmount.amount, new FloatRange(1, totemDef.MaxPurchaseableUpgrades(compLycanthrope)), "+" + upgradeAmount.amount, roundTo: 1f);

                bool canPurchase = totemDef.CanPurchase(compLycanthrope);
                string upgradeLabel = "Mashed_Bloodmoon_UpgradeLabel".Translate(compLycanthrope.usedTotemTracker.TryGetValue(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0), totemDef.purchaseHeartCost * upgradeAmount.amount);
                if (Widgets.ButtonText(upgradeButtonRect, upgradeLabel, true, canPurchase, active: canPurchase))
                {
                    totemDef.PurchaseTotemLevel(compLycanthrope, (int)upgradeAmount.amount);
                    foreach (Page_UpgradeBeastForm.UpgradeAmount val in upgradeAmountList)
                    {
                        val.amount = 1;
                    }
                }
            }
        }

        public static void DoRightRect(Rect inRect, LycanthropeTotemDef totemDef)
        {
            Widgets.DrawBoxSolidWithOutline(inRect, Widgets.WindowBGFillColor, Color.grey, 1);
            Rect mainRect = inRect.ContractedBy(Assets.RectPadding);
            GUI.DrawTexture(mainRect, ContentFinder<Texture2D>.Get(totemDef.backgroundTexPath));
            Rect iconRect = mainRect.ContractedBy(Assets.RectPadding / 2f);
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
