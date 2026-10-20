using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Dialog_InfoButtonPopup : Window
    {
        public override Vector2 InitialSize => new Vector2(600f, 400f);

        private readonly string description = "???";
        private Vector2 scroll;

        public Dialog_InfoButtonPopup(string desc)
        {
            description = desc;
            doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Rect infoRect = inRect;
            infoRect.height -= 40f;
            Widgets.LabelScrollable(infoRect.ContractedBy(GenUI.Pad), description, ref scroll);
        }
    }
}
