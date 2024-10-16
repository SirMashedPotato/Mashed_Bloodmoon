using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTRWorker_Gene : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (ModsConfig.BiotechActive)
            {
                if (geneDef != null && pawn.genes.HasActiveGene(geneDef))
                {
                    return true;
                }
                if (!geneDefs.NullOrEmpty())
                {
                    foreach (GeneDef def in geneDefs)
                    {
                        if (pawn.genes.HasActiveGene(def))
                        {
                            return true;
                        }
                    }
                }
            }
            return "Mashed_Bloodmoon_LTR_InvalidGene".Translate();
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (geneDef == null && geneDefs.NullOrEmpty())
            {
                yield return "both geneDef and geneDefs are null";
            }
            if (geneDef != null && !geneDefs.NullOrEmpty())
            {
                yield return "use either geneDef or geneDefs";
            }
        }

        public GeneDef geneDef;
        public List<GeneDef> geneDefs;
    }
}
