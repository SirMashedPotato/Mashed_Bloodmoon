using HarmonyLib;
using Verse;

namespace Mashed_Bloodmoon
{
    /// <summary>
    /// Prevents blood loss while the pawn has the Mashed_Bloodmoon_AdrenalineRush hediff.
    /// </summary>
    [HarmonyPatch(typeof(HediffGiver_Bleeding))]
    [HarmonyPatch("OnIntervalPassed")]
    public static class HediffGiver_Bleeding_OnIntervalPassed_Patch
    {
        public static bool Prefix(Pawn pawn)
        {
            return !pawn.health.hediffSet.HasHediff(HediffDefOf.Mashed_Bloodmoon_AdrenalineRush);
        }
    }
}
