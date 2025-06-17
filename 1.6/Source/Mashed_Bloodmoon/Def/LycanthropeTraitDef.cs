using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LycanthropeTraitDef : Def
    {
        [MustTranslate]
        public new string description;
        public TraitDef traitDef;
        public int traitDegree = 0;
        public int purchaseHeartCost = 0;

        /// <summary>
        /// Utility method to check if the pawn can purchase the trait
        /// </summary>
        public bool CanPurchase(HediffComp_Lycanthrope compLycanthrope)
        {
            return CanPurchase(compLycanthrope.usedTotemTracker.TryGetValue(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0));
        }

        /// <summary>
        /// Utility method to check if the pawn can purchase the trait
        /// </summary>
        public bool CanPurchase(int curHeartCount)
        {
            return curHeartCount >= purchaseHeartCost;
        }

        /// <summary>
        /// Utility method to check if the pawn already has the trait
        /// </summary>
        public bool HasTrait(Pawn pawn)
        {
            return pawn.story.traits.HasTrait(traitDef, traitDegree);
        }

        /// <summary>
        /// Utility method to check if the pawn can gain the trait
        /// </summary>
        public AcceptanceReport PawnRequirementsMet(Pawn pawn)
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

            return true;
        }

        /// <summary>
        /// Utility method for purchasing an trait
        /// </summary>
        public void PurchaseTrait(HediffComp_Lycanthrope compLycanthrope, Pawn pawn)
        {
            compLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts] -= purchaseHeartCost;
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
