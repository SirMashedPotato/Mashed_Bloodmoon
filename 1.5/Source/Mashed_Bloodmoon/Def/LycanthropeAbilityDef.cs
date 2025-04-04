﻿using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LycanthropeAbilityDef : Def
    {
        public AbilityDef abilityDef;
        public int heartCost = 0;
        public bool canBePurchased = true;

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
            return curHeartCount >= heartCost;
        }

        /// <summary>
        /// Utility method to check if the pawn can gain the ability
        /// </summary>
        public bool CanGainAbility(HediffComp_Lycanthrope compLycanthrope)
        {
            return !compLycanthrope.unlockedAbilityTracker.ContainsKey(this);
        }

        /// <summary>
        /// Utility method for purchasing an ability
        /// </summary>
        public void PurchaseAbility(HediffComp_Lycanthrope compLycanthrope)
        {
            compLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts] -= heartCost;
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
    }
}
