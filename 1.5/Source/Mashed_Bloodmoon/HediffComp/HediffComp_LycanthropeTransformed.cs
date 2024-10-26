using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
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
            try
            {
                base.CompPostTick(ref severityAdjustment);
                if (parent.pawn.IsHashIntervalTick(LycanthropeUtility.lycanthropeStressRate))
                {
                    if (currentStress >= StressMax)
                    {
                        if (inFury)
                        {
                            parent.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
                            parent.pawn.health.RemoveHediff(parent);
                            return;
                        }
                        StartFury();
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
            catch (Exception)
            {
                ///There's a chance for a ticking error after the hediff is removed, this should hopefully prevent that
            }
        }

        public void StartFury()
        {
            parent.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Mashed_Bloodmoon_LycanthropeFury, null, true, true);
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
            parent.pawn.abilities.GainAbility(AbilityDefOf.Mashed_Bloodmoon_ConsumeHeart);
            CompLycanthrope.LycanthropeTypeDef.transformationWorker?.PostTransformationBegin(parent.pawn);
            foreach (KeyValuePair<TotemTypeDef, int> usedTotem in CompLycanthrope.usedTotemTracker)
            {
                usedTotem.Key.transformationWorker?.PostTransformationBegin(parent.pawn, usedTotem.Value);
            }
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
            LycanthropeUtility.RemoveLinkedHediff(parent.pawn, HediffDefOf.Mashed_Bloodmoon_WolfsbloodRegeneration);
            parent.pawn.abilities.RemoveAbility(AbilityDefOf.Mashed_Bloodmoon_ConsumeHeart);
            CompLycanthrope.LycanthropeTypeDef.transformationWorker?.PostTransformationEnd(parent.pawn);
            foreach (KeyValuePair<TotemTypeDef, int> usedTotem in CompLycanthrope.usedTotemTracker)
            {
                usedTotem.Key.transformationWorker?.PostTransformationEnd(parent.pawn, usedTotem.Value);
            }
            int fatigueDuration = CurrentFatigueDuration();
            if (inFury)
            {
                fatigueDuration *= 2;
            }
            LycanthropeUtility.AddFatigueHediff(parent.pawn, fatigueDuration);
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
                icon = ContentFinder<Texture2D>.Get("UI/Gizmos/Mashed_Bloodmoon_TransformHuman", true),
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
            Scribe_Values.Look(ref currentStress, "currentStress", 0);
            Scribe_Values.Look(ref stressMax, "stressMax", 0);
        }
    }
}
