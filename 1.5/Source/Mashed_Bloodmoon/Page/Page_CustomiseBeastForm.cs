using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Page_CustomiseBeastForm : LycanthropePage
    {
        const float pawnZoom = 0.9f;
        Rot4 pawnRotation = new Rot4(2);

        ///Cached values
        LycanthropeTypeDef originalLycanthropeTypeDef;
        LycanthropeTransformationTypeDef originalLycanthropeTransformationTypeDef;
        Color originalPrimaryColour;
        Color originalSecondaryColour;
        Color originalTertiaryColour;

        List<FloatMenuOption> lycanthropeTypeOptions;
        List<FloatMenuOption> transformationTypeOptions;

        public override string PageTitle => "Mashed_Bloodmoon_CustomiseBeastForm".Translate().CapitalizeFirst() + ": " + pawn.NameShortColored;

        public Page_CustomiseBeastForm(HediffComp_Lycanthrope comp) : base(comp)
        {
            //TODO remove at some point
            if (compLycanthrope.TransformationTypeDef == null)
            {
                compLycanthrope.TransformationTypeDef = LycanthropeTransformationTypeDefOf.Mashed_Bloodmoon_Bloodmoon;
            }

            originalLycanthropeTypeDef = compLycanthrope.LycanthropeTypeDef;
            originalLycanthropeTransformationTypeDef = compLycanthrope.TransformationTypeDef;
            originalPrimaryColour = compLycanthrope.primaryColour;
            originalSecondaryColour = compLycanthrope.secondaryColour;
            originalTertiaryColour = compLycanthrope.tertiaryColour;
            Reset();
            lycanthropeTypeOptions = CacheLycanthropeTypeOptions();
            transformationTypeOptions = CacheTransformationTypeOptions();

        }

        public override void DoWindowContents(Rect inRect)
        {
            DrawPageTitle(inRect);
            inRect.yMin += rectLimitY;
            DoBottomButtons(inRect, "Accept".Translate(), "Mashed_Bloodmoon_UndoChanges".Translate(), midAct: Reset, showNext: true, doNextOnKeypress: false);

            Rect leftRect = inRect;
            leftRect.width = (inRect.width / 2) - (rectPadding / 2);
            leftRect.height -= rectLimitY;
            DoLeftSide(leftRect);

            Rect rightRect = leftRect;
            rightRect.x += rightRect.width + (rectPadding);
            DoRightSide(rightRect);
        }

        private void DoLeftSide(Rect inRect)
        {
            Rect primaryRect = inRect;
            primaryRect.height = (inRect.height / 3) - ((rectPadding / 3) * 2);
            Widgets.DrawMenuSection(primaryRect);
            DoColourSection(primaryRect, ref compLycanthrope.primaryColour, originalPrimaryColour, compLycanthrope.LycanthropeTypeDef.PrimaryColorDefault, "Mashed_Bloodmoon_CustomiseBeastForm_PrimaryLabel");

            Rect secondaryRect = primaryRect;
            secondaryRect.y += primaryRect.height + rectPadding;
            Widgets.DrawMenuSection(secondaryRect);
            DoColourSection(secondaryRect, ref compLycanthrope.secondaryColour, originalSecondaryColour, compLycanthrope.LycanthropeTypeDef.SecondaryColorDefault, "Mashed_Bloodmoon_CustomiseBeastForm_SecondaryLabel");

            Rect tertiaryRect = secondaryRect;
            tertiaryRect.y += secondaryRect.height + rectPadding;
            Widgets.DrawMenuSection(tertiaryRect);
            DoColourSection(tertiaryRect, ref compLycanthrope.tertiaryColour, originalTertiaryColour, compLycanthrope.LycanthropeTypeDef.TertiaryColorDefault, "Mashed_Bloodmoon_CustomiseBeastForm_TertiaryLabel");
        }

        private void DoColourSection(Rect mainRect, ref Color compColor, Color oldColor, Color defaultColor, string label)
        {
            Rect inRect = mainRect;
            inRect.x += rectPadding;
            inRect.y += rectPadding;
            inRect.width -= rectPadding;
            inRect.height -= rectPadding;

            Listing_Standard listing_Standard = new Listing_Standard
            {
                ColumnWidth = inRect.width / 2
            };
            listing_Standard.Begin(inRect);
            listing_Standard.Label(label.Translate());
            listing_Standard.Gap();
            DoColourLine(ref compColor.r, ref listing_Standard, "Red");
            DoColourLine(ref compColor.g, ref listing_Standard, "Green");
            DoColourLine(ref compColor.b, ref listing_Standard, "Blue");
            
            listing_Standard.End();

            Rect colorDisplayRect = inRect;
            colorDisplayRect.height *= 0.25f;
            colorDisplayRect.y += inRect.height - colorDisplayRect.height - rectPadding;
            ColorReadback(colorDisplayRect, defaultColor, oldColor, ref compColor);

            Rect newColorDisplayRect = inRect;
            newColorDisplayRect.x = (inRect.width / 2) + rectLimitY;
            newColorDisplayRect.width = (inRect.width / 2) - rectLimitY;
            newColorDisplayRect.height -= rectLimitY;
            Widgets.DrawBoxSolid(newColorDisplayRect, compColor);

            Rect randomiseRect = newColorDisplayRect;
            randomiseRect.height = colorDisplayRect.height / 2;
            randomiseRect.y = inRect.y + inRect.height - randomiseRect.height - rectPadding;

            if (Widgets.ButtonText(randomiseRect, "Random".Translate()))
            {
                compColor.r = Rand.Range(0f, 1f);
                compColor.g = Rand.Range(0f, 1f);
                compColor.b = Rand.Range(0f, 1f);
            }
        }

        private void DoColourLine(ref float compColor, ref Listing_Standard listing_Standard, string label)
        {
            compColor = (float)Math.Round(listing_Standard.SliderLabeled(label.Translate().CapitalizeFirst() + " (" + compColor.ToStringPercent() + ")", compColor, 0, 1) * 100) / 100;
        }

        /// <summary>
        /// Based on Dialog_GlowerColorPicker.ColorReadback
        /// </summary>
        private static void ColorReadback(Rect rect, Color defaultColor, Color oldColor, ref Color compColor)
        {
            rect.SplitVertically(rect.width / 2f, out Rect parent, out Rect parent2);
            RectDivider rectDivider = new RectDivider(parent, 195906069, null);
            TaggedString label = "Mashed_Bloodmoon_DefaultColor".Translate().CapitalizeFirst();
            TaggedString label2 = "OldColor".Translate().CapitalizeFirst();
            float width = Mathf.Max(new float[]
            {
                100f,
                label.GetWidthCached(),
                label2.GetWidthCached()
            });
            RectDivider rect2 = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            if (Widgets.ButtonText(rect2.NewCol(width, HorizontalJustification.Left), label))
            {
                compColor = defaultColor;
            }
            Widgets.DrawBoxSolid(rect2, defaultColor);

            RectDivider rect3 = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            if (Widgets.ButtonText(rect3.NewCol(width, HorizontalJustification.Left), label2))
            {
                compColor = oldColor;
            }
            Widgets.DrawBoxSolid(rect3, oldColor);

            RectDivider rect4 = new RectDivider(parent2, 195906069, null);
            rect4.NewCol(26f, HorizontalJustification.Left);
        }

        private void DoRightSide(Rect inRect)
        {
            Rect descriptionRect = inRect;
            descriptionRect.height = Text.LineHeight * 3.5f;
            DoDescriptionSection(descriptionRect);

            Rect previewRect = inRect;
            previewRect.height = (((inRect.height / 3) - ((rectPadding / 3) * 2)) * 2) + rectPadding;
            previewRect.y += descriptionRect.height + rectPadding;
            DoPreviewSection(previewRect);

            Rect buttonRect = previewRect;
            buttonRect.height = Text.LineHeight * 2f;
            buttonRect.y += previewRect.height + rectPadding;
            DoButtonSection(buttonRect);
        }

        private void DoPreviewSection(Rect mainRect)
        {
            Widgets.DrawMenuSection(mainRect);

            ///Pawn preview
            Rect pawnRect = mainRect;
            pawnRect.x += rectPadding;
            pawnRect.y += rectPadding;
            pawnRect.width -= (rectPadding * 2);
            pawnRect.height -= (rectPadding * 2);

            ///Thank god the ui pauses the game
            pawn.Drawer.renderer.SetAllGraphicsDirty();
            RenderTexture pawnImage = PortraitsCache.Get(pawn, new Vector2(pawnRect.width, pawnRect.height), pawnRotation, 
                cameraZoom: pawnZoom, supersample: true, compensateForUIScale: true, stylingStation: true);
            GUI.DrawTexture(pawnRect, pawnImage);
        }

        private void DoButtonSection(Rect mainRect)
        {
            ///Swap type button
            Rect lycanthropeTypeRect = mainRect;
            lycanthropeTypeRect.height = Text.LineHeight * 2;
            lycanthropeTypeRect.width /= 3;
            lycanthropeTypeRect.x += lycanthropeTypeRect.width;

            if (Widgets.ButtonText(lycanthropeTypeRect, "Mashed_Bloodmoon_SelectLycanthropeType".Translate().CapitalizeFirst()))
            {
                FloatMenu typeOptions = new FloatMenu(lycanthropeTypeOptions);
                Find.WindowStack.Add(typeOptions);
            }

            ///Rotate button left
            Rect rotateLeftRect = mainRect;
            rotateLeftRect.height = Text.LineHeight * 2;
            rotateLeftRect.width = rotateLeftRect.height;
            rotateLeftRect.x += rectPadding;

            if (Widgets.ButtonText(rotateLeftRect, "<"))
            {
                pawnRotation.Rotate(RotationDirection.Clockwise);
            }

            ///Rotate button right
            Rect rotateRightRect = rotateLeftRect;
            rotateRightRect.x = mainRect.x + mainRect.width - rotateRightRect.width - rectPadding;

            if (Widgets.ButtonText(rotateRightRect, ">"))
            {
                pawnRotation.Rotate(RotationDirection.Counterclockwise);
            }

            ///Transformation type button
            Rect transformationTypeRect = lycanthropeTypeRect;
            transformationTypeRect.y += rectPadding + transformationTypeRect.height;

            if (Widgets.ButtonText(transformationTypeRect, "Mashed_Bloodmoon_SelectTransformationType".Translate().CapitalizeFirst()))
            {
                FloatMenu typeOptions = new FloatMenu(transformationTypeOptions);
                Find.WindowStack.Add(typeOptions);
            }
        }

        private void DoDescriptionSection(Rect mainRect)
        {
            Widgets.DrawMenuSection(mainRect);
            Rect inRect = mainRect.ContractedBy(rectPadding);

            RectDivider rectDivider = new RectDivider(inRect, inRect.GetHashCode(), null);

            RectDivider rect1 = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            TaggedString label1 = "Mashed_Bloodmoon_Type".Translate() + ": " + compLycanthrope.LycanthropeTypeDef.LabelCap;
            Widgets.Label(rect1.NewCol(label1.GetWidthCached(), HorizontalJustification.Left), label1);

            RectDivider rect2 = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            TaggedString label2 = "Mashed_Bloodmoon_Artist".Translate() + ": " + compLycanthrope.LycanthropeTypeDef.artist;
            Widgets.Label(rect2.NewCol(label2.GetWidthCached(), HorizontalJustification.Left), label2);

            TaggedString label3 = "Mashed_Bloodmoon_Effect".Translate() + ": " + compLycanthrope.TransformationTypeDef.LabelCap;
            Widgets.Label(rect1.NewCol(label3.GetWidthCached(), HorizontalJustification.Right), label3);

            TaggedString label4 = "Source".Translate() + ": " + compLycanthrope.LycanthropeTypeDef.modContentPack.Name;
            Widgets.Label(rect2.NewCol(label4.GetWidthCached(), HorizontalJustification.Right), label4);
        }

        /// <summary>
        /// Player closed the menu without accepting changes, reset values in comp to cached values
        /// </summary>
        protected override void DoBack()
        {
            base.DoBack();
            compLycanthrope.LycanthropeTypeDef = originalLycanthropeTypeDef;
            compLycanthrope.TransformationTypeDef = originalLycanthropeTransformationTypeDef;
            compLycanthrope.primaryColour = originalPrimaryColour;
            compLycanthrope.secondaryColour = originalSecondaryColour;
            compLycanthrope.tertiaryColour = originalTertiaryColour;
        }

        /// <summary>
        /// Adds the dummy transformation hediff
        /// </summary>
        public override void PreOpen()
        {
            pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformedDummy);
            base.PreOpen();
        }

        /// <summary>
        /// Removes the dummy transformation hediff
        /// </summary>
        public override void PreClose()
        {
            pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformedDummy));
            base.PreClose();
        }

        /// <summary>
        /// Get list of all lycanthrope types that have been unlocked
        /// </summary>
        private List<FloatMenuOption> CacheLycanthropeTypeOptions()
        {
            lycanthropeTypeOptions = new List<FloatMenuOption>();

            foreach (LycanthropeTypeDef def in DefDatabase<LycanthropeTypeDef>.AllDefs)
            {
                FloatMenuOption item;
                AcceptanceReport acceptanceReport = def.PawnRequirementsMet(pawn);
                item = new FloatMenuOption(def.label.CapitalizeFirst(), delegate
                {
                    compLycanthrope.LycanthropeTypeDef = def;
                });

                if (!acceptanceReport.Accepted)
                {
                    item.Label += " (" + acceptanceReport.Reason + ")";
                    item.Disabled = true;
                }

                lycanthropeTypeOptions.Add(item);
            }
            return lycanthropeTypeOptions;
        }

        /// <summary>
        /// Get list of all transformation types that have been unlocked
        /// </summary>
        private List<FloatMenuOption> CacheTransformationTypeOptions()
        {
            transformationTypeOptions = new List<FloatMenuOption>();

            foreach (LycanthropeTransformationTypeDef def in DefDatabase<LycanthropeTransformationTypeDef>.AllDefs)
            {
                FloatMenuOption item;
                AcceptanceReport acceptanceReport = def.PawnRequirementsMet(pawn);
                item = new FloatMenuOption(def.label.CapitalizeFirst(), delegate
                {
                    compLycanthrope.TransformationTypeDef = def;
                });

                if (!acceptanceReport.Accepted)
                {
                    item.Label += " (" + acceptanceReport.Reason + ")";
                    item.Disabled = true;
                }

                transformationTypeOptions.Add(item);
            }
            return transformationTypeOptions;
        }

        private void Reset()
        {
            compLycanthrope.LycanthropeTypeDef = originalLycanthropeTypeDef;
            compLycanthrope.TransformationTypeDef = originalLycanthropeTransformationTypeDef;
            compLycanthrope.primaryColour = originalPrimaryColour;
            compLycanthrope.secondaryColour = originalSecondaryColour;
            compLycanthrope.tertiaryColour = originalTertiaryColour;
        }
    }
}
