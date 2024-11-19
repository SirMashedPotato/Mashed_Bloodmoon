using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LycanthropeAbilityDef : Def
    {
        public List<AbilityDef> abilityDefs;
        public int heartCost = 0;
        public float upgradeCostMult = 2f;
        public bool canBePurchased = true;

        /// <summary>
        /// Utility method to check if the pawn can upgrade the ability
        /// </summary>
        public bool CanUpgrade(int curLevel)
        {
            return curLevel < abilityDefs.Count;
        }

        /// <summary>
        /// Utility method to get the heart cost of upgrading an ability
        /// </summary>
        public float GetUpgradeCost(int curLevel)
        {
            return heartCost * (upgradeCostMult * (curLevel + 1));
        }
    }
}
