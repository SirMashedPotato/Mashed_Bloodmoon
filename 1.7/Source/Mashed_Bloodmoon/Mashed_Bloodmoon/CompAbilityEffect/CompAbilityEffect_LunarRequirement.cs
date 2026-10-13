using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_LunarRequirement : LycanthropeAbilityEffectComp
    {
        public new CompProperties_AbilityLunarRequirement Props => (CompProperties_AbilityLunarRequirement)props;

        public override bool GizmoDisabled(out string reason)
        {
            if (!RequirementsMet())
            {
                reason = "Mashed_Bloodmoon_AbilityLunarRequirementNotMet".Translate();
                return true;
            }
            return base.GizmoDisabled(out reason);
        }

        public bool RequirementsMet()
        {
            if (!parent.pawn.Spawned || parent.pawn.Map == null)
            {
                return false;
            }
            if (Props.beastHuntDef != null && Props.beastHuntDef.Completed(parent.pawn))
            {
                return true;
            }
            if (GenLocalDate.DayPercent(parent.pawn) < 0.25f || GenLocalDate.DayPercent(parent.pawn) > 0.8f)
            {
                return true;
            }
            foreach (GameConditionDef def in Props.gameConditions)
            {
                if (parent.pawn.Map.gameConditionManager.ConditionIsActive(def))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
