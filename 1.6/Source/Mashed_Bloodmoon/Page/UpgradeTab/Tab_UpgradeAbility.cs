using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Tab_UpgradeAbility
    {
        public static void DoCell(Rect leftRect, Rect rightRect, LycanthropeAbilityDef abilityDef, HediffComp_Lycanthrope compLycanthrope)
        {
            Pawn pawn = compLycanthrope.parent.pawn;
            DoLeftRect(leftRect, abilityDef, compLycanthrope, pawn);
            DoRightRect(rightRect, abilityDef);
        }

        public static void DoLeftRect(Rect inRect, LycanthropeAbilityDef abilityDef, HediffComp_Lycanthrope compLycanthrope, Pawn pawn)
        {
            AcceptanceReport acceptanceReport = abilityDef.PawnRequirementsMet(pawn);

            Widgets.DrawBoxSolidWithOutline(inRect, Widgets.WindowBGFillColor, Color.grey, 1);
            Rect mainRect = inRect.ContractedBy(Assets.RectPadding);
            RectDivider rectDivider = new RectDivider(mainRect, mainRect.GetHashCode(), null);

            RectDivider labelRect = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            Widgets.Label(labelRect.NewCol(abilityDef.LabelCap.GetWidthCached(), HorizontalJustification.Left), abilityDef.LabelCap);
            if (abilityDef.AlreadyUnlocked(compLycanthrope))
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

            Rect lowerRect = mainRect;
            lowerRect.height = Text.LineHeight * 1.5f;
            lowerRect.y = inRect.y + inRect.height - lowerRect.height - Assets.RectPadding;

            lowerRect.SplitVerticallyWithMargin(out Rect lowerInfoRect, out Rect lowerButtonRect, Assets.RectPadding);

            CompProperties_AbilityStressCost compStressCost = (CompProperties_AbilityStressCost)abilityDef.abilityDef.comps.Find(x => x is CompProperties_AbilityStressCost);
            if (compStressCost != null)
            {
                Widgets.Label(lowerInfoRect, "Mashed_Bloodmoon_AbilityStressCost".Translate(compStressCost.stressCost));
            }
            else
            {
                CompProperties_AbilityHeartCost compHeartCost = (CompProperties_AbilityHeartCost)abilityDef.abilityDef.comps.Find(x => x is CompProperties_AbilityHeartCost);
                if (compHeartCost != null)
                {
                    Widgets.Label(lowerInfoRect, "Mashed_Bloodmoon_AbilityHeartCost".Translate(compHeartCost.heartCost));
                }
            }

            Text.Font = font;

            if (!abilityDef.AlreadyUnlocked(compLycanthrope))
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
                    lowerButtonRect.width = 130f;
                    lowerButtonRect.x = inRect.x + inRect.width - lowerButtonRect.width - Assets.RectPadding;

                    bool canPurchase = abilityDef.CanPurchase(compLycanthrope);
                    string unlockLabel = "Mashed_Bloodmoon_UnlockLabel".Translate(compLycanthrope.usedTotemTracker.TryGetValue(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0), abilityDef.purchaseHeartCost); ;
                    if (Widgets.ButtonText(lowerButtonRect, unlockLabel, true, canPurchase, active: canPurchase))
                    {
                        abilityDef.Purchase(compLycanthrope);
                    }
                }
            }
        }

        public static void DoRightRect(Rect inRect, LycanthropeAbilityDef abilityDef)
        {
            Widgets.DrawBoxSolidWithOutline(inRect, Widgets.WindowBGFillColor, Color.grey, 1);
            Rect mainRect = inRect.ContractedBy(Assets.RectPadding);
            GUI.DrawTexture(mainRect, ContentFinder<Texture2D>.Get(abilityDef.backgroundTexPath));
            Rect iconRect = mainRect.ContractedBy(Assets.RectPadding / 2f);
            GUI.DrawTexture(iconRect, abilityDef.abilityDef.uiIcon);
        }
    }
}
