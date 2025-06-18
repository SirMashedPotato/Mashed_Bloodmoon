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

        public List<LycanthropeTypeRequirementWorker> requirementWorkers;
        public List<LycanthropeTransformationWorker> transformationWorkers;

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
            if (!requirementWorkers.NullOrEmpty())
            {
                string requirementsMet = "";
                foreach (LycanthropeTypeRequirementWorker requirementWorker in requirementWorkers)
                {
                    AcceptanceReport acceptanceReport = requirementWorker.PawnRequirementsMet(pawn);
                    if (!acceptanceReport)
                    {
                        if (requirementsMet != "")
                        {
                            requirementsMet += ", ";
                        }
                        requirementsMet += acceptanceReport.Reason;
                    }
                }
                if (requirementsMet != "")
                {
                    return requirementsMet;
                }
            }

            return true;
        }

        public virtual void PostTransformationBegin(Pawn pawn, int value = 0)
        {
            if (!transformationWorkers.NullOrEmpty())
            {
                foreach(LycanthropeTransformationWorker transformationWorker in transformationWorkers)
                {
                    transformationWorker.PostTransformationBegin(pawn, value);
                }
            }
        }

        public virtual void PostTransformationEnd(Pawn pawn, int value = 0)
        {
            if (!transformationWorkers.NullOrEmpty())
            {
                foreach (LycanthropeTransformationWorker transformationWorker in transformationWorkers)
                {
                    transformationWorker.PostTransformationEnd(pawn, value);
                }
            }
        }

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }

            if (!requirementWorkers.NullOrEmpty())
            {
                foreach (LycanthropeTypeRequirementWorker requirementWorker in requirementWorkers)
                {
                    foreach (string item in requirementWorker.ConfigErrors())
                    {
                        yield return item;
                    }
                }
            }

            if (!transformationWorkers.NullOrEmpty())
            {
                foreach(LycanthropeTransformationWorker transformationWorker in transformationWorkers)
                {
                    foreach (string item in transformationWorker.ConfigErrors())
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}
