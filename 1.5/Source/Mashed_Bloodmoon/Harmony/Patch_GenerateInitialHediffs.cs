using HarmonyLib;
using Verse;

namespace Mashed_Bloodmoon
{
    /// <summary>
    /// Just adds hediffs and abilities to pawns when they are generated, based on the presence of PawnLycanthropeProperties.
    /// Checks to see if the pawn already has the lycanthrope hediff, so it can be used in conjunction with the gene etc.
    /// </summary>
    [HarmonyPatch(typeof(PawnGenerator))]
    [HarmonyPatch("GenerateInitialHediffs")]
    public static class PawnGenerator_GenerateInitialHediffs_Patch
    {
        public static void Postfix(Pawn pawn)
        {
            PawnLycanthropeProperties props = PawnLycanthropeProperties.GetProps(pawn);
            if (props != null)
            {
                if (!LycanthropeUtility.PawnIsLycanthrope(pawn))
                {
                    pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_Lycanthrope);
                }

                HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);

                if (props.forcedTypeDef != null)
                {
                    compLycanthrope.LycanthropeTypeDef = props.forcedTypeDef;
                    compLycanthrope.ResetColours();
                }

                if (props.primaryColourOverride != null)
                {
                    compLycanthrope.primaryColour = props.primaryColourOverride;
                }

                if (props.secondaryColourOverride != null)
                {
                    compLycanthrope.secondaryColour = props.secondaryColourOverride;
                }

                if (props.tertiaryColourOverride != null)
                {
                    compLycanthrope.tertiaryColour = props.tertiaryColourOverride;
                }

                if (!props.startingTotemCounts.NullOrEmpty())
                {
                    foreach (PawnTotemRecord pawnTotemRecord in props.startingTotemCounts)
                    {
                        int value = pawnTotemRecord.startingCountRange.RandomInRange;
                        if (value > 0)
                        {
                            pawnTotemRecord.totemTypeDef.UseTotem(compLycanthrope, value);
                        }
                    }
                }

                if (!props.startingAbilities.NullOrEmpty())
                {
                    foreach(LycanthropeAbilityDef abilityDef in props.startingAbilities)
                    {
                        if (abilityDef.CanGainAbility(compLycanthrope))
                        {
                            abilityDef.UnlockAbility(compLycanthrope);
                        }
                    }
                }
            }
        }
    }
}
