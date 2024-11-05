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
    public class Page_CustomiseBeastForm : Page
    {
        HediffComp_Lycanthrope compLycanthrope;
        Pawn parent;

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
        Color eyeColour = Color.white; //TODO
        float eyeR;
        float eyeG;
        float eyeB;

        const float rectPadding = 12f;
        const float rectLimitY = 45f;

        List<FloatMenuOption> lycanthropeTypeOptions;

        public override string PageTitle => "Mashed_Bloodmoon_CustomiseBeastForm_Label".Translate().CapitalizeFirst() + ": " + parent.NameShortColored;

        public Page_CustomiseBeastForm(HediffComp_Lycanthrope comp) 
        {
            compLycanthrope = comp;
            parent = comp.parent.pawn;
            Reset();

            lycanthropeTypeOptions = CacheLycanthropeTypeOptions();
        }

        public override void DoWindowContents(Rect inRect)
        {
            DrawPageTitle(inRect);
            inRect.yMin += rectLimitY;
            DoBottomButtons(inRect, "Accept".Translate(), "Reset".Translate().CapitalizeFirst(), midAct: Reset, showNext: true, doNextOnKeypress: false);

            Rect leftRect = inRect;
            leftRect.width = (inRect.width / 2) - (rectPadding / 2);
            DoLeftSide(leftRect);

            Rect rightRect = leftRect;
            rightRect.x += rightRect.width + (rectPadding);
            rightRect.height -= (rectPadding * 4);
            DoRightSide(rightRect);
        }

        public void DoLeftSide(Rect inRect)
        {
            Rect primaryRect = inRect;
            primaryRect.height = (inRect.height / 3) - (rectPadding * 2);
            Widgets.DrawMenuSection(primaryRect);

            Rect secondaryRect = primaryRect;
            secondaryRect.y += primaryRect.height + rectPadding;
            Widgets.DrawMenuSection(secondaryRect);

            Rect eyeRect = secondaryRect;
            eyeRect.y += secondaryRect.height + rectPadding;
            Widgets.DrawMenuSection(eyeRect);
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
            compLycanthrope.eyeColour = eyeColour;

            ///TODO
            ///If the gizmo is only avaliable while transfromed
            ///Call transformation end in the original type def
            ///Then switch type
            ///Then call transformation started in the new type def

            base.DoNext();
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
                AcceptanceReport allowed = def.PawnRequirementsMet(parent);
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

            eyeColour = compLycanthrope.eyeColour;
            eyeR = eyeColour.r;
            eyeG = eyeColour.g;
            eyeB = eyeColour.b;
        }
    }
}
