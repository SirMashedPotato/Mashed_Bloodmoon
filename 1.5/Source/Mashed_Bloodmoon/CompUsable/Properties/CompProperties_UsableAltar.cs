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
        public bool allowTransformed = true;
    }
}
