using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTRWorker_Trait : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (traitDef != null && pawn.story.traits.HasTrait(traitDef))
            {
                return true;
            }

            if (!traitDefs.NullOrEmpty())
            {
                foreach (TraitDef def in traitDefs)
                {
                    if (pawn.story.traits.HasTrait(def))
                    {
                        return true;
                    }
                }
            }
            return "Mashed_Bloodmoon_LTR_InvalidTrait".Translate();
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (traitDef == null && traitDefs.NullOrEmpty())
            {
                yield return "both traitDef and traitDefs are null";
            }
            if (traitDef != null && !traitDefs.NullOrEmpty())
            {
                yield return "use either traitDef or traitDefs";
            }
        }

        public TraitDef traitDef;
        public List<TraitDef> traitDefs;
    }
}
