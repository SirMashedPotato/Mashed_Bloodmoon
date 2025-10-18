using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class FocusStrengthOffset_WolfsbloodAltar : FocusStrengthOffset
    {
        public float offsetPerFillLevel = 0.03f;

        public override string GetExplanation(Thing parent)
        {
            Building_WolfsbloodAltar wolfsbloodAltar = parent as Building_WolfsbloodAltar;
            return "Mashed_Bloodmoon_StatsReport_WolfsbloodAltar".Translate(wolfsbloodAltar.FillAmount().ToStringPercent()) + ": " + GetOffset(parent).ToStringWithSign("0%");
        }

        public override string GetExplanationAbstract(ThingDef def = null)
        {
            return "Mashed_Bloodmoon_StatsReport_WolfsbloodAltarAbstract".Translate(offsetPerFillLevel.ToStringPercent(), (offsetPerFillLevel * 10).ToStringPercent());
        }

        public override float GetOffset(Thing parent, Pawn user = null)
        {
            Building_WolfsbloodAltar wolfsbloodAltar = parent as Building_WolfsbloodAltar;
            return (wolfsbloodAltar.FillAmount() * 10) * offsetPerFillLevel;
        }

        public override bool CanApply(Thing parent, Pawn user = null)
        {
            if (parent.Spawned && parent is Building_WolfsbloodAltar wolfsbloodAltar)
            {
                return wolfsbloodAltar.FillAmount() > 0;
            }
            return false;
        }
    }
}
