using HarmonyLib;
using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    [HarmonyPatch(typeof(CompHoldingPlatformTarget))]
    [HarmonyPatch("CanBeCaptured", MethodType.Getter)]
    public static class CompHoldingPlatformTarget_CanBeCaptured_Patch
    {
        public static void Postfix(ref CompHoldingPlatformTarget __instance, ref bool __result)
        {
            if (__result)
            {
                if (!(__instance.parent is Pawn pawn))
                {
                    return;
                }

                if (!pawn.def.tradeTags.NullOrEmpty() && pawn.def.tradeTags.Contains("Mashed_Bloodmoon_FeralLycanthrope"))
                {
                    __result = false;
                }
            }
        }
    }
}
