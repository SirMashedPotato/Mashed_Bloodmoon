using Verse;

namespace Mashed_Bloodmoon
{
    public class CompUsable_WolfsbloodAltarFillBlood : CompUsable_WolfsbloodAltar
    {
        public override AcceptanceReport CanBeUsedBy(Pawn p, bool forced = false, bool ignoreReserveAndReachable = false)
        {
            Building_WolfsbloodAltar altar = parent as Building_WolfsbloodAltar;
            if (!altar.CanAddBlood())
            {
                return "Mashed_Bloodmoon_WolfsbloodAltarFull".Translate(altar);
            }

            return base.CanBeUsedBy(p, forced, ignoreReserveAndReachable);
        }
    }
}
