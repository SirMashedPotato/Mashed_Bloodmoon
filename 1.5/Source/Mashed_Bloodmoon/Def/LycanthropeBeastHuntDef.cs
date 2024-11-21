using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LycanthropeBeastHuntDef : Def
    {
        public ThingDef targetThingDef;
        public int consumeCount = 1;
        public LycanthropeTypeTransformationWorker transformationWorker;
        [NoTranslate]
        public string backgroundTexPath = "UI/Widgets/DesButBG";
        [NoTranslate]
        public string heartTexPath = "UI/Icons/Mashed_Bloodmoon_BeastHeart";
        [NoTranslate]
        public string consumedTexPath = "UI/Icons/Mashed_Bloodmoon_BeastHuntCompleted";

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }

            if (targetThingDef == null)
            {
                yield return "null thingDef";
            }

            if (transformationWorker != null)
            {
                foreach (string item in transformationWorker.ConfigErrors())
                {
                    yield return item;
                }
            }
        }

        public bool Completed(Pawn pawn)
        {
            return Completed(LycanthropeUtility.GetCompLycanthrope(pawn));
        }

        public bool Completed(HediffComp_Lycanthrope compLycanthrope)
        {
            return Completed(compLycanthrope.beastHuntTracker.TryGetValue(this, 0));
        }

        public bool Completed(int currentCount)
        {
            return currentCount >= consumeCount;
        }

        public int Progress(HediffComp_Lycanthrope compLycanthrope)
        {
            return compLycanthrope.beastHuntTracker.TryGetValue(this, 0);
        }

        public float CompletionProgress(HediffComp_Lycanthrope compLycanthrope)
        {
            return compLycanthrope.beastHuntTracker.TryGetValue(this, 0) / (float)consumeCount;
        }

        /// <summary>
        /// Utility method for consuming a great beast heart
        /// </summary>
        public void ConsumeBeastHeart(Pawn pawn)
        {
            ConsumeBeastHeart(LycanthropeUtility.GetCompLycanthrope(pawn), pawn);
        }

        /// <summary>
        /// Utility method for consuming a great beast heart
        /// </summary>
        public void ConsumeBeastHeart(HediffComp_Lycanthrope compLycanthrope, Pawn pawn)
        {
            if (!compLycanthrope.beastHuntTracker.ContainsKey(this))
            {
                compLycanthrope.beastHuntTracker.Add(this, 0);
            }

            if (Completed(compLycanthrope.beastHuntTracker[this]))
            {
                return;
            }

            compLycanthrope.beastHuntTracker[this]++;

            if (Completed(compLycanthrope.beastHuntTracker[this]))
            {
                pawn.records.Increment(RecordDefOf.Mashed_Bloodmoon_BeastHuntsCompleted);
                Messages.Message("Mashed_Bloodmoon_BeastHuntComplete".Translate(pawn, this), pawn, MessageTypeDefOf.PositiveEvent);
            }
        }
    }
}
