using HarmonyLib;
using Verse;

namespace Mashed_Bloodmoon
{
    [HarmonyPatch(typeof(PawnGenerator))]
    [HarmonyPatch("GenerateInitialHediffs")]
    public static class PawnGenerator_GenerateInitialHediffs_Patch
    {
        public static void Postfix(Pawn pawn)
        {
            PawnLycanthropeProperties props = PawnLycanthropeProperties.GetProps(pawn);
            props?.FillCompLycanthrope(pawn);
        }
    }
}
