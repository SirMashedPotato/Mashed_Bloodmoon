using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Page_CustomiseBeastForm : Page
    {
        HediffComp_Lycanthrope compLycanthrope;
        Pawn pawn;

        ///Temporary values
        LycanthropeTypeDef lycanthropeTypeDef;

        Color primaryColour = Color.white;
        float primaryR;
        float primaryG;
        float primaryB;
        Color secondaryColour = Color.white;
        float secondaryR;
        float secondaryG;
        float secondaryB;
        Color tertiaryColour = Color.white;
        float tertiaryR;
        float tertiaryG;
        float tertiaryB;

        const float rectPadding = 12f;
        const float rectLimitY = 45f;

        List<FloatMenuOption> lycanthropeTypeOptions;

        public override string PageTitle => "Mashed_Bloodmoon_CustomiseBeastForm_Label".Translate().CapitalizeFirst() + ": " + pawn.NameShortColored;

        public Page_CustomiseBeastForm(HediffComp_Lycanthrope comp) 
        {
            compLycanthrope = comp;
            pawn = comp.parent.pawn;
            Reset();

            lycanthropeTypeOptions = CacheLycanthropeTypeOptions();
        }

        public override void DoWindowContents(Rect inRect)
        {
            DrawPageTitle(inRect);
            inRect.yMin += rectLimitY;
            DoBottomButtons(inRect, "Accept".Translate(), "Reset".Translate(), midAct: Reset, showNext: true, doNextOnKeypress: false);

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
            Rect primaryRect = inRect;
            primaryRect.height = (inRect.height / 3) - ((rectPadding / 3) * 2);
            Widgets.DrawMenuSection(primaryRect);
            DoColourSection(primaryRect, ref primaryColour, compLycanthrope.primaryColour, lycanthropeTypeDef.PrimaryColorDefault,
                ref primaryR, ref primaryG, ref primaryB, "Mashed_Bloodmoon_CustomiseBeastForm_PrimaryLabel");

            Rect secondaryRect = primaryRect;
            secondaryRect.y += primaryRect.height + rectPadding;
            Widgets.DrawMenuSection(secondaryRect);
            DoColourSection(secondaryRect, ref secondaryColour, compLycanthrope.secondaryColour, lycanthropeTypeDef.SecondaryColorDefault,
                ref secondaryR, ref secondaryG, ref secondaryB, "Mashed_Bloodmoon_CustomiseBeastForm_SecondaryLabel");

            Rect tertiaryRect = secondaryRect;
            tertiaryRect.y += secondaryRect.height + rectPadding;
            Widgets.DrawMenuSection(tertiaryRect);
            DoColourSection(tertiaryRect, ref tertiaryColour, compLycanthrope.tertiaryColour, lycanthropeTypeDef.TertiaryColorDefault,
                ref tertiaryR, ref tertiaryG, ref tertiaryB, "Mashed_Bloodmoon_CustomiseBeastForm_TertiaryLabel");
        }

        public void DoColourSection(Rect mainRect, ref Color newColor, Color oldColor, Color defaultColor, ref float r, ref float g, ref float b, string label)
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
            DoColourLine(ref r, ref listing_Standard, "Red");
            DoColourLine(ref g, ref listing_Standard, "Green");
            DoColourLine(ref b, ref listing_Standard, "Blue");
            
            listing_Standard.End();

            Rect colorDisplayRect = inRect;
            colorDisplayRect.height = inRect.height * 0.25f;
            colorDisplayRect.y = inRect.y + inRect.height - colorDisplayRect.height - rectPadding;
            ColorReadback(colorDisplayRect, defaultColor, oldColor, ref r, ref g, ref b);

            Rect newColorDisplayRect = inRect;
            newColorDisplayRect.x = (inRect.width / 2) + rectLimitY;
            newColorDisplayRect.width = (inRect.width / 2) - rectLimitY;
            newColorDisplayRect.height -= rectLimitY;
            Widgets.DrawBoxSolid(newColorDisplayRect, newColor);

            Rect randomiseRect = newColorDisplayRect;
            randomiseRect.height = colorDisplayRect.height / 2;
            randomiseRect.y = inRect.y + inRect.height - randomiseRect.height - rectPadding;

            if (Widgets.ButtonText(randomiseRect, "Random".Translate()))
            {
                r = Rand.Range(0f, 1f);
                g = Rand.Range(0f, 1f);
                b = Rand.Range(0f, 1f);
            }

            newColor = new Color(r, g, b, 1);
        }

        public void DoColourLine(ref float color, ref Listing_Standard listing_Standard, string label)
        {
            color = (float)Math.Round(listing_Standard.SliderLabeled(label.Translate().CapitalizeFirst() + " (" + color.ToStringPercent() + ")", color, 0, 1) * 100) / 100;
        }

        /// <summary>
        /// Based on Dialog_GlowerColorPicker.ColorReadback
        /// </summary>
        private static void ColorReadback(Rect rect, Color defaultColor, Color oldColor, ref float r, ref float g, ref float b)
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
                r = defaultColor.r;
                g = defaultColor.g;
                b = defaultColor.b;
            }
            Widgets.DrawBoxSolid(rect2, defaultColor);

            RectDivider rect3 = rectDivider.NewRow(Text.LineHeight, VerticalJustification.Top);
            if (Widgets.ButtonText(rect3.NewCol(width, HorizontalJustification.Left), label2))
            {
                r = oldColor.r;
                g = oldColor.g;
                b = oldColor.b;
            }
            Widgets.DrawBoxSolid(rect3, oldColor);

            RectDivider rect4 = new RectDivider(parent2, 195906069, null);
            rect4.NewCol(26f, HorizontalJustification.Left);
        }

        public void DoRightSide(Rect inRect)
        {
            Widgets.DrawMenuSection(inRect);

            Rect setTypeRect = inRect;
            setTypeRect.height = Text.LineHeight * 2;
            setTypeRect.width = inRect.width / 3;
            setTypeRect.y += rectPadding;
            setTypeRect.x += setTypeRect.width;

            if (Widgets.ButtonText(setTypeRect, "Mashed_Bloodmoon_SelectLycanthropeType".Translate().CapitalizeFirst()))
            {
                FloatMenu typeOptions = new FloatMenu(lycanthropeTypeOptions);
                Find.WindowStack.Add(typeOptions);
            }
        }

        /// <summary>
        /// Applies changes
        /// </summary>
        protected override void DoNext()
        {
            compLycanthrope.LycanthropeTypeDef = lycanthropeTypeDef;
            compLycanthrope.primaryColour = primaryColour;
            compLycanthrope.secondaryColour = secondaryColour;
            compLycanthrope.tertiaryColour = tertiaryColour;

            base.DoNext();
        }

        protected override void DoBack()
        {
            Log.Message("test");
            base.DoBack();
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
        /// Get list of all types that have been unlocked
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
                    lycanthropeTypeDef = def;

                    primaryColour = compLycanthrope.primaryColour;
                    primaryR = def.PrimaryColorDefault.r;
                    primaryG = def.PrimaryColorDefault.g;
                    primaryB = def.PrimaryColorDefault.b;

                    secondaryColour = compLycanthrope.secondaryColour;
                    secondaryR = def.SecondaryColorDefault.r;
                    secondaryG = def.SecondaryColorDefault.g;
                    secondaryB = def.SecondaryColorDefault.b;

                    tertiaryColour = compLycanthrope.tertiaryColour;
                    tertiaryR = def.TertiaryColorDefault.r;
                    tertiaryG = def.TertiaryColorDefault.g;
                    tertiaryB = def.TertiaryColorDefault.b;
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

        private void Reset()
        {
            lycanthropeTypeDef = compLycanthrope.LycanthropeTypeDef;

            primaryColour = compLycanthrope.primaryColour;
            primaryR = primaryColour.r;
            primaryG = primaryColour.g;
            primaryB = primaryColour.b;

            secondaryColour = compLycanthrope.secondaryColour;
            secondaryR = secondaryColour.r;
            secondaryG = secondaryColour.g;
            secondaryB = secondaryColour.b;

            tertiaryColour = compLycanthrope.tertiaryColour;
            tertiaryR = tertiaryColour.r;
            tertiaryG = tertiaryColour.g;
            tertiaryB = tertiaryColour.b;
        }
    }
}
