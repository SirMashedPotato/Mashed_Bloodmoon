using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompUsable_WolfsbloodAltarTendWounds : CompUsable_WolfsbloodAltar
    {
        public override AcceptanceReport CanBeUsedBy(Pawn p, bool forced = false, bool ignoreReserveAndReachable = false)
        {
            if (!AbilityUtility.ValidateHasTendableWound(p, false, null))
            {
                return "AbilityMustHaveTendableWound".Translate(p);
            }

            return base.CanBeUsedBy(p, forced, ignoreReserveAndReachable);
        }
    }
}
