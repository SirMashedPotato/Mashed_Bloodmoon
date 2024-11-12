using HarmonyLib;
using Verse;

namespace Mashed_Bloodmoon
{
    /// <summary>
    /// Just adds hediffs and abilities to pawns when they are generated.
    /// </summary>
    [HarmonyPatch(typeof(PawnGenerator))]
    [HarmonyPatch("GenerateInitialHediffs")]
    public static class PawnGenerator_GenerateInitialHediffs_Patch
    {
        public static void Postfix(Pawn pawn)
        {
            PawnLycanthropeProperties props = PawnLycanthropeProperties.Get(pawn.kindDef) ?? PawnLycanthropeProperties.Get(pawn.def);
            if (props != null)
            {
                if (!LycanthropeUtility.PawnIsLycanthrope(pawn))
                {
                    pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_Lycanthrope);
                }

                if (props.startHidden)
                {
                    LycanthropeUtility.GetLycanthropeHediff(pawn).Severity = 0.1f;
                }

                HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);

                if (props.forcedTypeDef != null)
                {
                    compLycanthrope.LycanthropeTypeDef = props.forcedTypeDef;
                    compLycanthrope.ResetColours();
                }

                if (!props.startingTotemCounts.NullOrEmpty())
                {
                    foreach (PawnTotemRecord pawnTotemRecord in props.startingTotemCounts)
                    {
                        int value = pawnTotemRecord.startingCountRange.RandomInRange;
                        if (value > 0)
                        {
                            LycanthropeUtility.UseTotem(compLycanthrope, pawnTotemRecord.totemTypeDef, value);
                        }
                    }
                }
            }
        }
    }
}
