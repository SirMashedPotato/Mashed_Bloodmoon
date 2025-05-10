using Verse;

namespace Mashed_Bloodmoon
{
    public static class Compat_SimpleSidearms
    {
        /// <summary>
        /// Preventing transformed lycanthropes from equipping sidearms
        /// </summary>
        public static void StatCalculator_CanPickupSidearmType_Patch(Pawn pawn, ref string errString, ref bool __result)
        {
            if (__result && LycanthropeUtility.PawnIsTransformedLycanthrope(pawn))
            {
                __result = false;
                errString = "Mashed_Bloodmoon_LycanthropeCantDo".Translate(pawn);
            }
        }

        /// <summary>
        /// Hides the sidearms gizmo
        /// </summary>
        public static void Extensions_IsValidSidearmsCarrierRightNow_Patch(Pawn pawn, ref bool __result)
        {
            if (__result && LycanthropeUtility.PawnIsTransformedLycanthrope(pawn))
            {
                __result = false;
            }
        }
    }
}
