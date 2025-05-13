using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LycanthropeAbilityDef : Def
    {
        public new string description;
        public AbilityDef abilityDef;
        public int purchaseHeartCost = 0;
        public LycanthropeTypeRequirementWorker requirementWorker;

        /// <summary>
        /// Utility method to check if the pawn can purchase the ability
        /// </summary>
        public bool CanPurchase(HediffComp_Lycanthrope compLycanthrope)
        {
            return CanPurchase(compLycanthrope.usedTotemTracker.TryGetValue(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0));
        }

        /// <summary>
        /// Utility method to check if the pawn can purchase the ability
        /// </summary>
        public bool CanPurchase(int curHeartCount)
        {
            return curHeartCount >= purchaseHeartCost;
        }

        /// <summary>
        /// Utility method to check if the pawn can gain the ability
        /// </summary>
        public bool HasAbility(HediffComp_Lycanthrope compLycanthrope)
        {
            return compLycanthrope.unlockedAbilityTracker.ContainsKey(this);
        }

        /// <summary>
        /// Utility method for purchasing an ability
        /// </summary>
        public void PurchaseAbility(HediffComp_Lycanthrope compLycanthrope)
        {
            compLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts] -= purchaseHeartCost;
            UnlockAbility(compLycanthrope);
        }

        /// <summary>
        /// Utility method for unlocking an ability
        /// </summary>
        public void UnlockAbility(HediffComp_Lycanthrope compLycanthrope)
        {

            compLycanthrope.unlockedAbilityTracker.Add(this, 1);
            Messages.Message("Mashed_Bloodmoon_AbilityUnlocked".Translate(compLycanthrope.parent.pawn, this), compLycanthrope.parent.pawn, MessageTypeDefOf.PositiveEvent);
        }

        public AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (requirementWorker != null)
            {
                return requirementWorker.PawnRequirementsMet(pawn);
            }
            return true;
        }

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }

            if (requirementWorker != null)
            {
                foreach (string item in requirementWorker.ConfigErrors())
                {
                    yield return item;
                }
            }
        }
    }
}
