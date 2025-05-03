using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    /// <summary>
    /// Increase damage taken if the source is made out of silver
    /// </summary>
    [HarmonyPatch(typeof(Verb_MeleeAttackDamage))]
    [HarmonyPatch("DamageInfosToApply")]
    public static class Verb_MeleeAttackDamage_DamageInfosToApply_Patch
    {
        public static void Postfix(LocalTargetInfo target, ref Verb_MeleeAttackDamage __instance, ref IEnumerable<DamageInfo> __result)
        {
            if (__result.EnumerableNullOrEmpty())
            {
                return;
            }

            if (target.Pawn == null)
            {
                return;
            }

            float pawnSilverWeakness = target.Pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropeSilverWeakness);
            if (pawnSilverWeakness <= 0)
            {
                return;
            }

            float silverDamageFactor = GetDamageFactor(ref __instance);
            if (silverDamageFactor == 0)
            {
                return;
            }

            List<DamageInfo> newResult = new List<DamageInfo>();
            foreach (DamageInfo dinfo in __result) 
            {
                dinfo.SetAmount(LycanthropeUtility.LycanthropeSilverDamageFactor(dinfo.Amount, pawnSilverWeakness, silverDamageFactor));
                newResult.Add(dinfo);
            }

            __result = newResult;
        }

        private static float GetDamageFactor(ref Verb_MeleeAttackDamage __instance)
        {
            if (__instance.EquipmentSource != null)
            {
                return __instance.EquipmentSource.GetStatValue(StatDefOf.Mashed_Bloodmoon_SilverDamageFactor);
            }

            return 0;
        }
    }
}
