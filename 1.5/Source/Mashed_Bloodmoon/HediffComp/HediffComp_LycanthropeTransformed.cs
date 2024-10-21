using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffComp_LycanthropeTransformed : HediffComp
    {
        public bool inFury = false;
        public int currentStress;
        private int stressMax = -1;
        private Gizmo_LycanthropeStress lycanthropeStressGizmo;
        private HediffComp_Lycanthrope compLycanthrope;

        public HediffCompProperties_LycanthropeTransformed Props => (HediffCompProperties_LycanthropeTransformed)props;

        public HediffComp_Lycanthrope CompLycanthrope
        {
            get
            {
                if (compLycanthrope == null)
                {
                    compLycanthrope = LycanthropeUtility.GetCompLycanthrope(parent.pawn);
                }
                return compLycanthrope;
            }
        }

        public int StressMax
        {
            get
            {
                if (stressMax == -1)
                {
                    stressMax = (int)parent.pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropicStressMax);
                }
                return stressMax;
            }
        }

        public int CurrentFatigueDuration() => (int)(currentStress * LycanthropeUtility.lycanthropeStressToTicks);

        public float CurrentTransformedDuration() => currentStress * LycanthropeUtility.lycanthropeStressRate;

        /// <summary>
        /// Ticking stress
        /// </summary>
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (parent.pawn.IsHashIntervalTick(LycanthropeUtility.lycanthropeStressRate))
            {
                if (currentStress >= StressMax)
                {
                    if (inFury)
                    {
                        parent.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
                        inFury = false;
                        parent.pawn.health.RemoveHediff(parent);
                        return;
                    }
                    StartFury("Mashed_Bloodmoon_FuryReasonStress".Translate());
                }
                else
                {
                    if (inFury)
                    {
                        currentStress += 2;
                    }
                    else
                    {
                        currentStress++;
                    }
                }
            }
        }

        public void StartFury(string reason = "???")
        {
            parent.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Mashed_Bloodmoon_LycanthropeFury, reason, true, true);
            currentStress = 0;
            inFury = true;
        }

        /// <summary>
        /// Adding linked hediff
        /// </summary>
        public override void CompPostMake()
        {
            base.CompPostMake();
            currentStress = 0;
            parent.pawn.records.Increment(RecordDefOf.Mashed_Bloodmoon_TransformationCount);
            LycanthropeUtility.AddLinkedHediff(parent.pawn, HediffDefOf.Mashed_Bloodmoon_LycanthropeClaws, RimWorld.BodyPartDefOf.Hand);
            LycanthropeUtility.AddLinkedHediff(parent.pawn, HediffDefOf.Mashed_Bloodmoon_LycanthropeTeeth, BodyPartDefOf.Jaw);
            CompLycanthrope.LycanthropeTypeDef.transformationWorker?.PostTransformationBegin(parent.pawn);
        }

        /// <summary>
        /// Removing linked hediff
        /// Adding fatigue hediff
        /// </summary>
        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            LycanthropeUtility.RemoveLinkedHediff(parent.pawn, HediffDefOf.Mashed_Bloodmoon_LycanthropeClaws);
            LycanthropeUtility.RemoveLinkedHediff(parent.pawn, HediffDefOf.Mashed_Bloodmoon_LycanthropeTeeth);
            LycanthropeUtility.AddFatigueHediff(parent.pawn, CurrentFatigueDuration());
            CompLycanthrope.LycanthropeTypeDef.transformationWorker?.PostTransformationEnd(parent.pawn);
        }

        /// <summary>
        /// 
        /// </summary>
        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            yield return new Command_Action
            {
                defaultLabel = "Mashed_Bloodmoon_TransformHuman_Label".Translate(),
                defaultDesc = "Mashed_Bloodmoon_TransformHuman_Desc".Translate(parent.pawn, CurrentFatigueDuration().ToStringTicksToPeriod()),
                action = delegate ()
                {
                    parent.pawn.health.RemoveHediff(parent.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformed));
                },
                Disabled = false,
            };

            if (lycanthropeStressGizmo == null)
            {
                lycanthropeStressGizmo = new Gizmo_LycanthropeStress(this);
            }
            yield return lycanthropeStressGizmo;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CompExposeData()
        {
            Scribe_Values.Look(ref currentStress, "Mashed_Bloodmoon_LycanthropicStress", 0);
        }
    }
}
