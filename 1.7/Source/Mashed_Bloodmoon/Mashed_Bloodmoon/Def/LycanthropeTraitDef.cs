using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LycanthropeTraitDef : LycanthropeDef
    {
        public TraitDef traitDef;
        public int traitDegree = 0;
        
        public bool AlreadyUnlocked(Pawn pawn)
        {
            return pawn.story.traits.HasTrait(traitDef, traitDegree);
        }

        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            foreach (Trait trait in pawn.story.traits.allTraits)
            {
                bool conflictsFlag = false;

                if (trait.def == traitDef)
                {
                    conflictsFlag = true;
                }

                if (!traitDef.conflictingTraits.NullOrEmpty() && traitDef.ConflictsWith(trait))
                {
                    conflictsFlag = true;
                }

                if(!trait.def.exclusionTags.NullOrEmpty() && !traitDef.exclusionTags.NullOrEmpty())
                {
                    if (trait.def.exclusionTags.Intersect(traitDef.exclusionTags).Any())
                    {
                        conflictsFlag = true;
                    }
                }

                if (conflictsFlag)
                {
                    return "Mashed_Bloodmoon_ConflictingTrait".Translate(trait.LabelCap);
                }
            }

            return base.PawnRequirementsMet(pawn);
        }

        public override void Purchase(HediffComp_Lycanthrope compLycanthrope)
        {
            base.Purchase(compLycanthrope);
            Pawn pawn = compLycanthrope.Pawn;
            Trait trait = new Trait(traitDef, traitDegree, true);
            pawn.story.traits.GainTrait(trait);
            Messages.Message("Mashed_Bloodmoon_GainedTrait".Translate(pawn, trait.LabelCap), compLycanthrope.parent.pawn, MessageTypeDefOf.PositiveEvent);
        }

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }

            if (traitDef == null)
            {
                yield return "traitDef is null";
            }
        }
    }
}
