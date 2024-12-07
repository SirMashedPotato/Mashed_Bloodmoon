using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using static UnityEngine.Random;

namespace Mashed_Bloodmoon
{
    public class Page_UpgradeBeastForm : LycanthropePage
    {
        public override string PageTitle => "Mashed_Bloodmoon_UpgradeBeastForm".Translate().CapitalizeFirst() + ": " + pawn.NameShortColored;

        private static readonly Texture2D ConsumedHeartsFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.7f, 0.3f, 0.3f));
        private readonly List<LycanthropeTotemDef> TotemList;
        private readonly List<LycanthropeAbilityDef> AbilityList;
        private static Vector2 scrollPositionTotem = Vector2.zero;
        private static Vector2 scrollPositionAbility = Vector2.zero;

        public float innerRectHeightTotem;
        public float innerRectHeightAbility;
        public static float rowHeight = Text.LineHeight * 6f;

        public Page_UpgradeBeastForm(HediffComp_Lycanthrope comp) : base(comp)
        {
            TotemList = DefDatabase<LycanthropeTotemDef>.AllDefsListForReading.Where(x => x.displayAsTotem).ToList();
            AbilityList = DefDatabase<LycanthropeAbilityDef>.AllDefsListForReading;
            innerRectHeightTotem = TotemList.Count * (rowHeight + (rectPadding / 2f));
            innerRectHeightAbility = AbilityList.Count * (rowHeight + (rectPadding / 2f));
        }

        public override void DoWindowContents(Rect inRect)
        {
            DrawPageTitle(inRect);
            Widgets.ButtonImage(new Rect(inRect.width - 30f, 0f, 30f, 30f), TexButton.Info, true, "Mashed_Bloodmoon_UpgradeBeastFormDesc".Translate());
            //if(Widgets.ButtonImage(new Rect(inRect.width - 30f, 0f, 30f, 30f), TexButton.Info, true, "Mashed_Bloodmoon_UpgradeBeastFormDesc".Translate(pawn)))
            //{
            //display new page with list of all stat bonuses. use string maker, set scroll bar size based on string size.
            //}

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
            Widgets.FillableBar(inRect, fillPercent, ConsumedHeartsFillTex, Texture2D.grayTexture, true);
            var anchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(inRect, LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.label + " (" + consumedCount + " / " + maxLevel + ")");
            Text.Anchor = anchor;
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

            DoTotemRightRect(leftRect, totemDef);
            DoTotemLeftRect(rightRect, totemDef);
        }

        public void DoTotemRightRect(Rect inRect, LycanthropeTotemDef totemDef)
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
                float statValue = compLycanthrope.usedTotemTracker.TryGetValue(totemDef, 0) * totemDef.statIncreasePerLevel;
                RectDivider statRect = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
                TaggedString statLabel = " - " + statDef.LabelCap + ": " + statValue.ToStringWithSign();
                if (!totemDef.onlyTransformed)
                {
                    statLabel += " " + "Mashed_Bloodmoon_TotemActiveWhileHuman".Translate();
                }
                Widgets.Label(statRect.NewCol(statLabel.GetWidthCached(), HorizontalJustification.Left), statLabel);
            }
            Text.Font = font;

            if (totemDef.canBePurchased)
            {
                Rect upgradeRect = mainRect;
                upgradeRect.height = Text.LineHeight * 1.5f;
                upgradeRect.width = 130f;
                upgradeRect.y = inRect.y + inRect.height - upgradeRect.height - rectPadding;
                upgradeRect.x = inRect.x + inRect.width - upgradeRect.width - rectPadding;
                bool canPurchase = totemDef.CanUpgrade(compLycanthrope);
                string upgradeLabel = "Mashed_Bloodmoon_UpgradeTotemLabel".Translate(compLycanthrope.usedTotemTracker.TryGetValue(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0), totemDef.purchaseHeartCost);
                Color textColor = canPurchase ? Color.cyan : Color.red;
                if (Widgets.ButtonText(upgradeRect, upgradeLabel, true, canPurchase, textColor, active: canPurchase))
                {
                    totemDef.PurchaseTotemLevel(compLycanthrope);
                }
            }
        }

        public void DoTotemLeftRect(Rect inRect, LycanthropeTotemDef totemDef)
        {
            Widgets.DrawMenuSection(inRect);
            Rect mainRect = inRect.ContractedBy(rectPadding);
        }

        public void DoRightSide(Rect inRect)
        {
            Widgets.DrawMenuSection(inRect);
        }

        public void DoAbilityRow(Rect inRect, LycanthropeAbilityDef abilityDef)
        {

        }
    }
}
