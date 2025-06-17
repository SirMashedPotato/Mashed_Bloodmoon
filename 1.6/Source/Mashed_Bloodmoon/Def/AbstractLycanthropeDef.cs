using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public abstract class LycanthropeDef : Def
    {
        [MustTranslate]
        public new string description;
        [MustTranslate]
        public string extraTooltip;

        public int purchaseHeartCost = 0;

        public LycanthropeTypeRequirementWorker requirementWorker;
        public LycanthropeTransformationWorker transformationWorker;

        public virtual bool AlreadyUnlocked(HediffComp_Lycanthrope compLycanthrope)
        {
            return false;
        }

        public virtual bool CanPurchase(HediffComp_Lycanthrope compLycanthrope)
        {
            return CanPurchase(compLycanthrope.usedTotemTracker.TryGetValue(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0));
        }

        public virtual bool CanPurchase(int curHeartCount)
        {
            return curHeartCount >= purchaseHeartCost;
        }

        public virtual void Purchase(HediffComp_Lycanthrope compLycanthrope)
        {
            Purchase(compLycanthrope, 1);
        }

        public virtual void Purchase(HediffComp_Lycanthrope compLycanthrope, int count)
        {
            compLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts] -= purchaseHeartCost * count;
        }

        public virtual void Unlock(HediffComp_Lycanthrope compLycanthrope)
        {

        }

        public virtual AcceptanceReport PawnRequirementsMet(Pawn pawn)
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

            if (transformationWorker != null)
            {
                foreach (string item in transformationWorker.ConfigErrors())
                {
                    yield return item;
                }
            }
        }
    }
}
