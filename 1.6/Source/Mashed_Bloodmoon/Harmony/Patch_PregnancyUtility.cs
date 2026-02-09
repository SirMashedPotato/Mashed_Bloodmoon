using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    /// <summary>
    /// Patch to make children lycanthropes if parent/s are lycanthropes
    /// </summary>
    [HarmonyPatch(typeof(PregnancyUtility))]
    [HarmonyPatch("ApplyBirthOutcome")]
    public static class PregnancyUtility_ApplyBirthOutcome_Patch
    {
        private const float BloodmoonChance = 0.25f;
        private const float FatherChance = 0.25f;
        private const float MotherChance = 0.5f;

        public static void Postfix(ref Thing __result, float quality, Pawn geneticMother, Pawn father = null)
        {
            if (father == null)
            {
                return;
            }

            if (__result == null)
            {
                return;
            }

            if (__result is Pawn child && !child.Dead)
            {
                float chance = 0f;

                if (geneticMother != null && LycanthropeUtility.PawnIsLycanthrope(geneticMother))
                {
                    chance += MotherChance;
                }

                if (father != null && LycanthropeUtility.PawnIsLycanthrope(father))
                {
                    chance += FatherChance;
                }

                if (Mashed_Bloodmoon_ModSettings.HuntsmanMoon_ChildbirthLycanthropy)
                {
                    if (child.Map != null && child.Map.gameConditionManager.ActiveConditions.Where(x => x is GameCondition_HuntsmansMoon).Any())
                    {
                        chance += BloodmoonChance;
                    }
                }

                if (Rand.Chance(chance))
                {
                    if (!LycanthropeUtility.PawnIsLycanthrope(child))
                    {
                        child.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_Lycanthrope);
                    }
                }
            }
        }
    }
}