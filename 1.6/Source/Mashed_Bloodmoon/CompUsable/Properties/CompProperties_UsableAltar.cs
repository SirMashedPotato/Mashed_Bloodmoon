using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompProperties_UsableAltar : CompProperties_Usable
    {
        [MustTranslate]
        public string gizmoDescription = "";
        [NoTranslate]
        public string gizmoTexPath;
        public bool compUseEffects = false;
        public bool onlyLycanthrope = true;
        public bool onlyHuman = false;
        public bool allowTransformed = true;
        public int heartCost = 0;
        public float bloodCost;
    }
}
