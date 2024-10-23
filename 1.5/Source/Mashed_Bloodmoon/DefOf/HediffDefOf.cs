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
        ///General
        public static HediffDef Mashed_Bloodmoon_SaniesLupinus;
        public static HediffDef Mashed_Bloodmoon_LycanthropeDormant;
        public static HediffDef Mashed_Bloodmoon_Lycanthrope;
        public static HediffDef Mashed_Bloodmoon_LycanthropeTransformed;
        public static HediffDef Mashed_Bloodmoon_LycanthropeFatigue;
        public static HediffDef Mashed_Bloodmoon_LycanthropeClaws;
        public static HediffDef Mashed_Bloodmoon_LycanthropeTeeth;

        ///Wolfsbane
        public static HediffDef Mashed_Bloodmoon_WolfsbaneResistance;
        public static HediffDef Mashed_Bloodmoon_WolfsbanePrevention;
        public static HediffDef Mashed_Bloodmoon_WolfsbaneNausea;
        ///Wolfsblood
        public static HediffDef Mashed_Bloodmoon_WolfsbloodAdrenaline;
        public static HediffDef Mashed_Bloodmoon_WolfsbloodRegeneration;
    }
}