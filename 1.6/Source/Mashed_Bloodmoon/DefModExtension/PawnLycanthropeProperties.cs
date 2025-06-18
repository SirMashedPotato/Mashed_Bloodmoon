using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class PawnLycanthropeProperties : DefModExtension
    {
        public float chance = 1f;
        public float transformOnDamageChanceOverride = 0.1f;
        public LycanthropeBeastFormDef forcedTypeDef = null;
        public LycanthropeTransformationTypeDef forcedTransformationTypeDef = null;
        public LycanthropeClawTypeDef forcedClawTypeDef = null;
        public Color? primaryColourOverride = null;
        public Color? secondaryColourOverride = null;
        public Color? tertiaryColourOverride = null;
        public List<PawnTotemRecord> startingTotemCounts;
        public List<LycanthropeAbilityDef> startingAbilities;
        public List<LycanthropeClawTypeDef> startingClaws;

        public static PawnLycanthropeProperties Get(Def def) => def.GetModExtension<PawnLycanthropeProperties>();

        /// <summary>
        /// Returns PawnLycanthropeProperties, checked from all valid source defs, in order of priority.
        /// </summary>
        public static PawnLycanthropeProperties GetProps(Pawn pawn)
        {
            if (pawn.story?.Adulthood != null)
            {
                PawnLycanthropeProperties props = Get(pawn.story.Adulthood);
                if (props != null)
                {
                    return props;
                }
            }

            if (pawn.story?.Childhood != null)
            {
                PawnLycanthropeProperties props = Get(pawn.story.Childhood);
                if (props != null)
                {
                    return props;
                }
            }

            return Get(pawn.kindDef) ?? Get(pawn.def) ?? null;
        }

        /// <summary>
        /// Just adds hediffs, abilities, etc to pawns when they are generated.
        /// Checks to see if the pawn already has the lycanthrope hediff, so it can be used in conjunction with the gene etc.
        /// </summary>
        public void FillCompLycanthrope(Pawn pawn)
        {
            if (!LycanthropeUtility.PawnIsLycanthrope(pawn))
            {
                pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_Lycanthrope);
            }

            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);

            if (forcedTypeDef != null)
            {
                compLycanthrope.BeastFormDef = forcedTypeDef;
                compLycanthrope.ResetColours();
            }

            if (forcedTransformationTypeDef != null)
            {
                compLycanthrope.TransformationTypeDef = forcedTransformationTypeDef;
            }

            if (forcedClawTypeDef != null)
            {
                compLycanthrope.equippedClawType = forcedClawTypeDef;
            }

            if (primaryColourOverride != null)
            {
                compLycanthrope.primaryColour = (Color)primaryColourOverride;
            }

            if (secondaryColourOverride != null)
            {
                compLycanthrope.secondaryColour = (Color)secondaryColourOverride;
            }

            if (tertiaryColourOverride != null)
            {
                compLycanthrope.tertiaryColour = (Color)tertiaryColourOverride;
            }

            if (!startingTotemCounts.NullOrEmpty())
            {
                foreach (PawnTotemRecord pawnTotemRecord in startingTotemCounts)
                {
                    int value = pawnTotemRecord.startingCountRange.RandomInRange;
                    if (value > 0)
                    {
                        pawnTotemRecord.totemTypeDef.Upgrade(compLycanthrope, value, false);
                    }
                }
            }

            if (!startingAbilities.NullOrEmpty())
            {
                foreach (LycanthropeAbilityDef abilityDef in startingAbilities)
                {
                    if (!abilityDef.AlreadyUnlocked(compLycanthrope))
                    {
                        abilityDef.Unlock(compLycanthrope);
                    }
                }
            }

            if (!startingClaws.NullOrEmpty())
            {
                foreach (LycanthropeClawTypeDef clawDef in startingClaws)
                {
                    if (!clawDef.AlreadyUnlocked(compLycanthrope))
                    {
                        clawDef.Unlock(compLycanthrope);
                    }
                }
            }
        }
    }
}
