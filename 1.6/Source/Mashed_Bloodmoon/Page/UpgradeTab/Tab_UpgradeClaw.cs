using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public static class Tab_UpgradeClaw
    {
        public static void DoCell(Rect inRect, LycanthropeClawTypeDef clawDef, HediffComp_Lycanthrope compLycanthrope)
        {
            Pawn pawn = compLycanthrope.parent.pawn;
            DoRect(inRect, clawDef, compLycanthrope, pawn);
        }

        public static void DoRect(Rect inRect, LycanthropeClawTypeDef clawDef, HediffComp_Lycanthrope compLycanthrope, Pawn pawn)
        {
            AcceptanceReport acceptanceReport = clawDef.PawnRequirementsMet(pawn);

            Widgets.DrawBoxSolidWithOutline(inRect, Widgets.WindowBGFillColor, Color.grey, 1);
            Rect mainRect = inRect.ContractedBy(Assets.RectPadding);
            RectDivider rectDivider = new RectDivider(mainRect, mainRect.GetHashCode(), null);

            RectDivider labelRect = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            Widgets.Label(labelRect.NewCol(clawDef.LabelCap.GetWidthCached(), HorizontalJustification.Left), clawDef.LabelCap);

            if (compLycanthrope.equippedClawType == clawDef)
            {
                TaggedString levelLabel = "Equipped".Translate();
                Widgets.Label(labelRect.NewCol(levelLabel.GetWidthCached(), HorizontalJustification.Right), levelLabel);
            }
            else if (clawDef.HasClaw(compLycanthrope))
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
            Widgets.Label(descRect, clawDef.description);
            Text.Font = font;

           

            if (compLycanthrope.equippedClawType == clawDef)
            {
                return;
            }

            Rect upgradeRect = mainRect;
            upgradeRect.height = Text.LineHeight * 1.5f;
            upgradeRect.width = 130f;
            upgradeRect.y = inRect.y + inRect.height - upgradeRect.height - Assets.RectPadding;
            upgradeRect.x = inRect.x + inRect.width - upgradeRect.width - Assets.RectPadding;

            if (compLycanthrope.unlockedClawTracker.Contains(clawDef))
            {
                if (Widgets.ButtonText(upgradeRect, "Mashed_Bloodmoon_Equip".Translate(), true))
                {
                    compLycanthrope.equippedClawType = clawDef;
                }
                return;
            }

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

            if(clawDef.purchaseHeartCost > 0)
            {
                bool canPurchase = clawDef.CanPurchase(compLycanthrope);
                string unlockLabel = "Mashed_Bloodmoon_UnlockLabel".Translate(compLycanthrope.usedTotemTracker.TryGetValue(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0), clawDef.purchaseHeartCost); ;
                if (Widgets.ButtonText(upgradeRect, unlockLabel, true, canPurchase, active: canPurchase))
                {
                    clawDef.PurchaseClaw(compLycanthrope);
                }
            }
        }
    }
}
