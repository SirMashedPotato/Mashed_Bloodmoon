﻿using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    [DefOf]
    public static class HediffDefOf
    {
        static HediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(HediffDefOf));
        }
        public static HediffDef Mashed_Bloodmoon_Lycanthrope;
        public static HediffDef Mashed_Bloodmoon_LycanthropeTransformed;
        public static HediffDef Mashed_Bloodmoon_LycanthropeExhaustion;
    }
}