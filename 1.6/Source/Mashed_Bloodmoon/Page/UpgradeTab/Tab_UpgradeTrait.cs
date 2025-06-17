using RimWorld;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public static class Tab_UpgradeTrait
    {
        public static void DoCell(Rect inRect, LycanthropeTraitDef traitDef, HediffComp_Lycanthrope compLycanthrope)
        {
            Pawn pawn = compLycanthrope.parent.pawn;
            DoRect(inRect, traitDef, compLycanthrope, pawn);
        }

        public static void DoRect(Rect inRect, LycanthropeTraitDef traitDef, HediffComp_Lycanthrope compLycanthrope, Pawn pawn)
        {
            AcceptanceReport acceptanceReport = traitDef.PawnRequirementsMet(pawn);

            Widgets.DrawBoxSolidWithOutline(inRect, Widgets.WindowBGFillColor, Color.grey, 1);
            Rect mainRect = inRect.ContractedBy(Assets.RectPadding);
            RectDivider rectDivider = new RectDivider(mainRect, mainRect.GetHashCode(), null);

            RectDivider labelRect = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            string label = traitDef.traitDef.DataAtDegree(traitDef.traitDegree).GetLabelCapFor(pawn);
            Widgets.Label(labelRect.NewCol(label.GetWidthCached(), HorizontalJustification.Left), label);

            if (traitDef.AlreadyUnlocked(pawn))
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
            string description = traitDef.description ?? traitDef.traitDef.DataAtDegree(traitDef.traitDegree).description;

            Widgets.Label(descRect, description.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn).Resolve());
            Text.Font = font;

            if (traitDef.AlreadyUnlocked(pawn))
            {
                return;
            }

            Rect upgradeRect = mainRect;
            upgradeRect.height = Text.LineHeight * 1.5f;
            upgradeRect.width = 130f;
            upgradeRect.y = inRect.y + inRect.height - upgradeRect.height - Assets.RectPadding;
            upgradeRect.x = inRect.x + inRect.width - upgradeRect.width - Assets.RectPadding;

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
            else
            {
                bool canPurchase = traitDef.CanPurchase(compLycanthrope);
                string unlockLabel = "Mashed_Bloodmoon_UnlockLabel".Translate(compLycanthrope.usedTotemTracker.TryGetValue(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0), traitDef.purchaseHeartCost); ;
                if (Widgets.ButtonText(upgradeRect, unlockLabel, true, canPurchase, active: canPurchase))
                {
                    traitDef.Purchase(compLycanthrope);
                }
            }
        }
    }
}
