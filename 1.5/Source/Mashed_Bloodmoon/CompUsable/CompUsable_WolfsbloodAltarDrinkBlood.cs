using Verse;

namespace Mashed_Bloodmoon
{
    public class CompUsable_WolfsbloodAltarDrinkBlood : CompUsable_WolfsbloodAltar
    {
        public override AcceptanceReport CanBeUsedBy(Pawn p, bool forced = false, bool ignoreReserveAndReachable = false)
        {
            Building_WolfsbloodAltar altar = parent as Building_WolfsbloodAltar;
            if (!altar.CanConsumeBlood())
            {
                return "Mashed_Bloodmoon_WolfsbloodAltarEmpty".Translate(altar);
            }

            return base.CanBeUsedBy(p, forced, ignoreReserveAndReachable);
        }
    }
}
