using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Page_UpgradeBeastForm : LycanthropePage
    {
        public override string PageTitle => "Mashed_Bloodmoon_UpgradeBeastForm".Translate().CapitalizeFirst() + ": " + pawn.NameShortColored;

        private readonly List<LycanthropeTotemDef> TotemList;
        private readonly List<LycanthropeAbilityDef> AbilityList;
        private static Vector2 scrollPositionTotem = Vector2.zero;
        private static Vector2 scrollPositionAbility = Vector2.zero;

        public float innerRectHeightTotem;
        public float innerRectHeightAbility;
        public static float rowHeight = Text.LineHeight * 5f;

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
            if(Widgets.ButtonImage(new Rect(inRect.width - 30f, 0f, 30f, 30f), TexButton.Info, true, "Mashed_Bloodmoon_UpgradeBeastFormDesc".Translate(pawn)))
            {
                //display new page with list of all stat bonuses. use string maker, set scroll bar size based on string size.
            }

            inRect.yMin += rectLimitY;
            DoBottomButtons(inRect, showNext: false);

            Rect leftRect = inRect;
            leftRect.width = (inRect.width / 2) - (rectPadding / 2);
            leftRect.height -= rectLimitY;
            DoLeftSide(leftRect);

            Rect rightRect = leftRect;
            rightRect.x += rightRect.width + (rectPadding);
            DoRightSide(rightRect);
        }

        public void DoLeftSide(Rect inRect)
        {
            Rect totemRowRect = inRect;
            totemRowRect.height = rowHeight;
            DoTotemRow(totemRowRect, LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts);

            Rect scrollRect = inRect;
            scrollRect.height -= totemRowRect.height + (rectPadding / 2f);
            scrollRect.y += totemRowRect.height + (rectPadding / 2f);

            Rect innerRect = scrollRect;
            innerRect.height = innerRectHeightTotem;
            innerRect.width -= 30f;

            Widgets.BeginScrollView(scrollRect, ref scrollPositionTotem, innerRect);
            int index = 0;
            foreach (LycanthropeTotemDef totemDef in TotemList)
            {
                Rect greatBeastRect = innerRect;
                greatBeastRect.height = rowHeight;
                greatBeastRect.y += (((rectPadding / 2f) + rowHeight) * index);
                DoTotemRow(greatBeastRect, totemDef);
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

            foreach (StatDef statDef in totemDef.statDefs)
            {
                float statValue = compLycanthrope.usedTotemTracker.TryGetValue(totemDef, 0) * totemDef.statIncreasePerLevel;
                RectDivider statRect = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
                TaggedString statLabel = " - " + statDef.LabelCap + ": " + statValue.ToStringWithSign();
                Widgets.Label(statRect.NewCol(statLabel.GetWidthCached(), HorizontalJustification.Left), statLabel);
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
