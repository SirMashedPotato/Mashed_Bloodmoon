using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LycanthropeBeastHuntDef : Def
    {
        [MustTranslate]
        public new string description;
        public BeastHuntType beastHuntType = BeastHuntType.Heart;
        public ThingDef targetThingDef;
        public PawnKindDef targetKindDef;
        public int targetCount = 1;
        public bool startHidden = false;
        public int anomalyLevelToReveal = 0;
        public LycanthropeBeastHuntCompletionWorker completionWorker;
        public LycanthropeTransformationWorker transformationWorker;
        [NoTranslate]
        public string backgroundTexPath = "UI/Widgets/DesButBG";
        [NoTranslate]
        public string heartTexPath = "UI/Icons/Mashed_Bloodmoon_BeastHeart";
        [NoTranslate]
        public string completedTexPath = "UI/Icons/Mashed_Bloodmoon_BeastHuntCompleted";
        [MustTranslate]
        public string extraTooltip;

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }

            if (completionWorker != null)
            {
                foreach (string item in completionWorker.ConfigErrors())
                {
                    yield return item;
                }
            }

            if (transformationWorker != null)
            {
                foreach (string item in transformationWorker.ConfigErrors())
                {
                    yield return item;
                }
            }

            if (targetThingDef != null && targetKindDef != null)
            {
                yield return "Only use one of targetThingDef and targetKindDef";
            }

            if (startHidden && anomalyLevelToReveal > 0)
            {
                yield return "Only use one of startHidden and anomalyLevelToReveal";
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
            return currentCount >= targetCount;
        }

        public int Progress(HediffComp_Lycanthrope compLycanthrope)
        {
            return compLycanthrope.beastHuntTracker.TryGetValue(this, 0);
        }

        public float CompletionProgress(HediffComp_Lycanthrope compLycanthrope)
        {
            return compLycanthrope.beastHuntTracker.TryGetValue(this, 0) / (float)targetCount;
        }

        public bool IsHidden(HediffComp_Lycanthrope compLycanthrope)
        {
            return startHidden && Progress(compLycanthrope) == 0;
        }

        public bool AnomalyIsHidden()
        {
            return ModsConfig.AnomalyActive && anomalyLevelToReveal > 0
                && Find.Storyteller.difficulty.AnomalyPlaystyleDef == AnomalyPlaystyleDefOf.Standard
                && Find.Anomaly.Level < anomalyLevelToReveal;
        }

        /// <summary>
        /// Utility method for progressing a beast hunt
        /// </summary>
        public void ProgressBeastHunt(Pawn pawn)
        {
            ProgressBeastHunt(LycanthropeUtility.GetCompLycanthrope(pawn), pawn);
        }

        /// <summary>
        /// Utility method for progressing a beast hunt
        /// </summary>
        public void ProgressBeastHunt(HediffComp_Lycanthrope compLycanthrope, Pawn pawn)
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
                compLycanthrope.completedBeastHunts++;
                pawn.records.Increment(RecordDefOf.Mashed_Bloodmoon_BeastHuntsCompleted);
                completionWorker?.PostBeastHuntCompleted(compLycanthrope, pawn);
                Messages.Message("Mashed_Bloodmoon_BeastHuntComplete".Translate(pawn, this), pawn, MessageTypeDefOf.PositiveEvent);
            }
        }
    }
}
