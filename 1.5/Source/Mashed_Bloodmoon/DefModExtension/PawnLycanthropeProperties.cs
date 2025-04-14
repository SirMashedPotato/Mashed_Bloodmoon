using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class PawnLycanthropeProperties : DefModExtension
    {
        public float chance = 1f;
        public float transformOnDamageChanceOverride = 0.1f;
        public LycanthropeTypeDef forcedTypeDef;
        public LycanthropeTransformationTypeDef forcedTransformationTypeDef;
        public Color primaryColourOverride;
        public Color secondaryColourOverride;
        public Color tertiaryColourOverride;
        public List<PawnTotemRecord> startingTotemCounts;
        public List<LycanthropeAbilityDef> startingAbilities;

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
    }
}
