using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class PawnLycanthropeProperties : DefModExtension
    {
        public float chance = 1f;
        public bool startHidden = false;
        public LycanthropeTypeDef forcedTypeDef;
        public List<PawnTotemRecord> startingTotemCounts;

        public static PawnLycanthropeProperties Get(Def def) => def.GetModExtension<PawnLycanthropeProperties>();
    }
}
