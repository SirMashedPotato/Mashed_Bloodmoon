using RimWorld;
using System.Collections.Generic;
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
            if (!traitDef.conflictingTraits.NullOrEmpty())
            {
                foreach(Trait trait in pawn.story.traits.allTraits)
                {
                    if (traitDef.ConflictsWith(trait))
                    {
                        return "Mashed_Bloodmoon_ConflictingTrait".Translate(trait.LabelCap);
                    }
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
