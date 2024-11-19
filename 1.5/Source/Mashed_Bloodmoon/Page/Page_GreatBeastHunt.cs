using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Page_GreatBeastHunt : LycanthropePage
    {
        public override string PageTitle => "Mashed_Bloodmoon_GreatBeastHunt".Translate().CapitalizeFirst() + ": " + pawn.NameShortColored;

        public Page_GreatBeastHunt(HediffComp_Lycanthrope comp) : base(comp)
        {

        }

        public override void DoWindowContents(Rect inRect)
        {
            DrawPageTitle(inRect);
            inRect.yMin += rectLimitY;
            DoBottomButtons(inRect, showNext: false);
        }
    }
}
