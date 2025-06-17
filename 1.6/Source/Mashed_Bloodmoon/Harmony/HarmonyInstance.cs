using HarmonyLib;
using Verse;
using System.Reflection;
using RimWorld;

namespace Mashed_Bloodmoon
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            Harmony harmony = new Harmony("com.Mashed_Bloodmoon");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            harmony.Patch(AccessTools.Method(typeof(EquipmentUtility), nameof(EquipmentUtility.CanEquip), new[] { typeof(Thing), typeof(Pawn), typeof(string).MakeByRefType(), typeof(bool) }),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(HarmonyPatches.EquipmentUtility_CanEquip_Patch)));

            if (ModsConfig.IsActive("petetimessix.simplesidearms"))
            {
                harmony.Patch(AccessTools.Method(AccessTools.TypeByName("PeteTimesSix.SimpleSidearms.Utilities.StatCalculator"), "CanPickupSidearmType"), postfix: new HarmonyMethod(typeof(Compat_SimpleSidearms), nameof(Compat_SimpleSidearms.StatCalculator_CanPickupSidearmType_Patch)));
                harmony.Patch(AccessTools.Method(AccessTools.TypeByName("PeteTimesSix.SimpleSidearms.Extensions"), "IsValidSidearmsCarrierRightNow"), postfix: new HarmonyMethod(typeof(Compat_SimpleSidearms), nameof(Compat_SimpleSidearms.Extensions_IsValidSidearmsCarrierRightNow_Patch)));
            }
        }
    }

    public static class HarmonyPatches
    {
        /// <summary>
        /// Prevents transformed lycanthropes from equipping weapons
        /// </summary>
        public static void EquipmentUtility_CanEquip_Patch(Pawn pawn, ref string cantReason, ref bool __result)
        {
            if (__result && LycanthropeUtility.PawnIsTransformedLycanthrope(pawn))
            {
                __result = false;
                cantReason = "Mashed_Bloodmoon_LycanthropeCantDo".Translate(pawn);
            }
        }
    }
}
