using HarmonyLib;
using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    /// <summary>
    /// Prevents ideo roles with skill requirements from being removed when a lycanthrope enters beast form
    /// Side effect that a lycanthrope in beast form can then be given any role with a skill requirement, even if they normally wouldn't meet it
    /// </summary>
    [HarmonyPatch(typeof(RoleRequirement_MinSkillAny))]
    [HarmonyPatch("Met")]
    public static class RoleRequirement_MinSkillAny_Met_Patch
    {
        public static void Postfix(Pawn p, ref bool __result)
        {
            if (!__result)
            {
                if (LycanthropeUtility.PawnIsTransformedLycanthrope(p))
                {
                    __result = true;
                }
            }
        }
    }

    /// <summary>
    /// Prevents transformed lycanthropes being valid ritual targets for the role change ritual
    /// </summary>
    [HarmonyPatch(typeof(RitualRoleIdeoRoleChanger))]
    [HarmonyPatch("AppliesToPawn")]
    public static class RitualRoleIdeoRoleChanger_AppliesToPawn_Patch
    {
        public static void Postfix(Pawn p, ref string reason, ref bool __result)
        {
            if (__result)
            {
                if (LycanthropeUtility.PawnIsTransformedLycanthrope(p))
                {
                    __result = false;
                    reason = "Mashed_Bloodmoon_InBeastForm".Translate(p);
                }
            }
        }
    }
}
