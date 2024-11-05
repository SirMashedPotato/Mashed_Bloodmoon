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
        Color tertiaryColour = Color.white; //TODO
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
            DoColourSection(primaryRect, ref primaryColour, compLycanthrope.primaryColour, ref primaryR, ref primaryG, ref primaryB, "Mashed_Bloodmoon_CustomiseBeastForm_PrimaryLabel");

            Rect secondaryRect = primaryRect;
            secondaryRect.y += primaryRect.height + rectPadding;
            Widgets.DrawMenuSection(secondaryRect);
            DoColourSection(secondaryRect, ref secondaryColour, compLycanthrope.secondaryColour, ref secondaryR, ref secondaryG, ref secondaryB, "Mashed_Bloodmoon_CustomiseBeastForm_SecondaryLabel");

            Rect tertiaryRect = secondaryRect;
            tertiaryRect.y += secondaryRect.height + rectPadding;
            Widgets.DrawMenuSection(tertiaryRect);
            DoColourSection(tertiaryRect, ref tertiaryColour, compLycanthrope.tertiaryColour, ref tertiaryR, ref tertiaryG, ref tertiaryB, "Mashed_Bloodmoon_CustomiseBeastForm_TertiaryLabel");
        }

        public void DoColourSection(Rect mainRect, ref Color newColor, Color oldColor, ref float r, ref float g, ref float b, string label)
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
            newColor = new Color(r, g, b, 1);
            listing_Standard.End();
        }

        public void DoColourLine(ref float color, ref Listing_Standard listing_Standard, string label)
        {
            color = (float)Math.Round(listing_Standard.SliderLabeled(label.Translate().CapitalizeFirst() + " (" + color.ToStringPercent() + ")", color, 0, 1) * 100) / 100;
        }

        public void DoRightSide(Rect inRect)
        {
            Widgets.DrawMenuSection(inRect);
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

            ///TODO
            ///If the gizmo is only avaliable while transfromed
            ///Call transformation end in the original type def
            ///Then switch type
            ///Then call transformation started in the new type def

            base.DoNext();
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
                AcceptanceReport allowed = def.PawnRequirementsMet(pawn);
                if (allowed)
                {
                    item = new FloatMenuOption(def.label, delegate
                    {
                        lycanthropeTypeDef = def;
                        primaryColour = def.graphicData.color;
                        secondaryColour = def.graphicData.colorTwo;
                    });
                    lycanthropeTypeOptions.Add(item);
                }
                /*
                else
                {
                    item = new FloatMenuOption(def.label + " (" + allowed.Reason + ")", delegate
                    {

                    });
                    lycanthropeTypeOptions.Add(item);
                }
                */
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
