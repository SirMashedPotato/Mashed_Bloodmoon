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
        public Color primaryColourOverride;
        public Color secondaryColourOverride;
        public Color tertiaryColourOverride;
        public List<PawnTotemRecord> startingTotemCounts;
        public List<LycanthropeAbilityDef> startingAbilities;

        public static PawnLycanthropeProperties Get(Def def) => def.GetModExtension<PawnLycanthropeProperties>();

        public static PawnLycanthropeProperties GetProps(Pawn pawn) => Get(pawn.kindDef) ?? Get(pawn.def) ?? null;
    }
}
