using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class RequirementWorker_Trait : LycanthropeRequirementWorker
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
                return "Mashed_Bloodmoon_RequirementWorker_MissingOneOf".Translate() + DoMissingList(traitDefs);
            }
            return "Mashed_Bloodmoon_RequirementWorker_Missing".Translate(traitDef);
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
