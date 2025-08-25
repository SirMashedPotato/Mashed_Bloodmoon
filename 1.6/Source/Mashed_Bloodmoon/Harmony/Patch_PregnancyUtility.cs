using HarmonyLib;
using RimWorld;
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