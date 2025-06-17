using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public abstract class LycanthropePage : Page
    {
        public const float rectLimitY = 45f;
        public HediffComp_Lycanthrope compLycanthrope;
        public Pawn pawn;

        public LycanthropePage(HediffComp_Lycanthrope comp)
        {
            compLycanthrope = comp;
            pawn = comp.parent.pawn;
        }
    }
}
