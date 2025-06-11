using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LycanthropeClawTypeDef : Def
    {
        [MustTranslate]
        public new string description;
        public HediffDef clawHediffDef;
        public int purchaseHeartCost = 0;
        public LycanthropeTypeRequirementWorker requirementWorker;

        /// <summary>
        /// Utility method to check if the pawn can purchase the claws
        /// </summary>
        public bool CanPurchase(HediffComp_Lycanthrope compLycanthrope)
        {
            return CanPurchase(compLycanthrope.usedTotemTracker.TryGetValue(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0));
        }

        /// <summary>
        /// Utility method to check if the pawn can purchase the claws
        /// </summary>
        public bool CanPurchase(int curHeartCount)
        {
            return curHeartCount >= purchaseHeartCost;
        }

        /// <summary>
        /// Utility method to check if the pawn can gain the claws
        /// </summary>
        public bool HasAbility(HediffComp_Lycanthrope compLycanthrope)
        {
            return compLycanthrope.unlockedClawTracker.Contains(this);
        }

        /// <summary>
        /// Utility method for purchasing an claws
        /// </summary>
        public void PurchaseAbility(HediffComp_Lycanthrope compLycanthrope)
        {
            compLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts] -= purchaseHeartCost;
            UnlockClaws(compLycanthrope);
        }

        /// <summary>
        /// Utility method for unlocking an claws
        /// </summary>
        public void UnlockClaws(HediffComp_Lycanthrope compLycanthrope)
        {

            compLycanthrope.unlockedClawTracker.Add(this);
            Messages.Message("Mashed_Bloodmoon_ClawsUnlocked".Translate(compLycanthrope.parent.pawn, this), compLycanthrope.parent.pawn, MessageTypeDefOf.PositiveEvent);
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

            if (clawHediffDef == null)
            {
                yield return "clawHediffDef is null";
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
