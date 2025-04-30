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
        public List<Hediff> linkedHediffs;

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
            set { 
                stressMax = value;
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

        /// <summary>
        /// 
        /// </summary>
        public void StartFury()
        {
            parent.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Mashed_Bloodmoon_LycanthropeFury, null, true, true);
            currentStress = 0;
            inFury = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CompPostMake()
        {
            base.CompPostMake();
            currentStress = 0;
            parent.pawn.records.Increment(RecordDefOf.Mashed_Bloodmoon_TransformationCount);

            linkedHediffs = new List<Hediff> { };
            AddLinkedHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeClaws, RimWorld.BodyPartDefOf.Hand);
            AddLinkedHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeTeeth, BodyPartDefOf.Jaw);

            LycanthropeUtility.MoveEquippedToInventory(parent.pawn);
        }

        /// <summary>
        /// Transformation workers are called here instead of in CompPostMake due to potential null references when trying to fetch this comp
        /// </summary>
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);

            //TODO remove at some point
            if (CompLycanthrope.TransformationTypeDef == null)
            {
                CompLycanthrope.TransformationTypeDef = LycanthropeTransformationTypeDefOf.Mashed_Bloodmoon_Bloodmoon;
            }
            CompLycanthrope.TransformationTypeDef.PlayTransformationStartEffects(parent.pawn);

            CompLycanthrope.LycanthropeTypeDef.transformationWorker?.PostTransformationBegin(parent.pawn);
            foreach (KeyValuePair<LycanthropeAbilityDef, int> unlockedAbility in CompLycanthrope.unlockedAbilityTracker)
            {
                parent.pawn.abilities.GainAbility(unlockedAbility.Key.abilityDef);
            }
            foreach (KeyValuePair<LycanthropeTotemDef, int> usedTotem in CompLycanthrope.usedTotemTracker)
            {
                usedTotem.Key.transformationWorker?.PostTransformationBegin(parent.pawn, usedTotem.Value);
            }
            foreach (KeyValuePair<LycanthropeBeastHuntDef, int> greatBeastDef in CompLycanthrope.beastHuntTracker)
            {
                if (greatBeastDef.Key.Completed(greatBeastDef.Value))
                {
                    greatBeastDef.Key.transformationWorker?.PostTransformationBegin(parent.pawn);
                }
            }
        }

        /// <summary>
        /// Adds a linked hediff
        /// </summary>
        public void AddLinkedHediff(Hediff hediff)
        {
            linkedHediffs.Add(hediff);
        }

        /// <summary>
        /// Adds a linked hediff
        /// </summary>
        public void AddLinkedHediff(HediffDef hediffDef, BodyPartRecord bodyPartRecord = null)
        {
            Hediff hediff = HediffMaker.MakeHediff(hediffDef, parent.pawn, bodyPartRecord);
            parent.pawn.health.AddHediff(hediff);
            AddLinkedHediff(hediff);
        }

        /// <summary>
        /// Adds a linked hediff to all body parts of a specific def
        /// </summary>
        public void AddLinkedHediff(HediffDef hediffDef, BodyPartDef partDef)
        {
            foreach (BodyPartRecord bodyPartRecord in parent.pawn.health.hediffSet.GetNotMissingParts())
            {
                if (bodyPartRecord.def == partDef)
                {
                    AddLinkedHediff(hediffDef, bodyPartRecord);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();

            //TODO remove at some point
            if (CompLycanthrope.TransformationTypeDef == null)
            {
                CompLycanthrope.TransformationTypeDef = LycanthropeTransformationTypeDefOf.Mashed_Bloodmoon_Bloodmoon;
            }
            CompLycanthrope.TransformationTypeDef.PlayTransformationEndEffects(parent.pawn);

            RemoveLinkedHediffs();
            CompLycanthrope.LycanthropeTypeDef.transformationWorker?.PostTransformationEnd(parent.pawn);
            foreach (KeyValuePair<LycanthropeAbilityDef, int> unlockedAbility in CompLycanthrope.unlockedAbilityTracker)
            {
                parent.pawn.abilities.RemoveAbility(unlockedAbility.Key.abilityDef);
            }
            foreach (KeyValuePair<LycanthropeTotemDef, int> usedTotem in CompLycanthrope.usedTotemTracker)
            {
                usedTotem.Key.transformationWorker?.PostTransformationEnd(parent.pawn, usedTotem.Value);
            }
            foreach (KeyValuePair<LycanthropeBeastHuntDef, int> greatBeastDef in CompLycanthrope.beastHuntTracker)
            {
                if (greatBeastDef.Key.Completed(greatBeastDef.Value))
                {
                    greatBeastDef.Key.transformationWorker?.PostTransformationEnd(parent.pawn);
                }
            }

            int fatigueDuration = CurrentFatigueDuration();
            if (inFury)
            {
                fatigueDuration *= 2;
            }
            LycanthropeUtility.AddFatigueHediff(parent.pawn, fatigueDuration);
            parent.pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformationEnd);
        }

        /// <summary>
        /// Removes all linked hediffs
        /// </summary>
        public void RemoveLinkedHediffs()
        {
            foreach (Hediff hediff in linkedHediffs)
            {
                hediff?.pawn.health.RemoveHediff(hediff);
            }
            linkedHediffs.Clear();
        }

        /// <summary>
        /// Doing this here instead of using HediffCompProperties_DisappearsOnDeath avoids an out of bounds error if the pawn dies while transformed
        /// </summary>
        public override void Notify_PawnKilled()
        {
            base.Notify_PawnKilled();
            parent.pawn.health.RemoveHediff(parent);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
        {
            if (victim != null)
            {
                parent.pawn.records.Increment(RecordDefOf.Mashed_Bloodmoon_PawnsKilledTransformed);
                LycanthropeUtility.ProgressBeastHunts(parent.pawn, victim.def, BeastHuntType.Kill);
                if (Rand.Chance(parent.pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropeKillStressReductionChance)))
                {
                    currentStress -= Mathf.Clamp((int)(StressMax * 0.9f), 0, currentStress);
                    MoteMaker.MakeAttachedOverlay(parent.pawn, ThingDefOf.Mashed_Bloodmoon_TransformEffect, Vector3.zero, parent.pawn.DrawSize.y);
                    Messages.Message("Mashed_Bloodmoon_StressReduced".Translate(parent.pawn), parent.pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
            base.Notify_KilledPawn(victim, dinfo);
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
        public override string CompDescriptionExtra
        {
            get
            {
                string tooltip = "\n";

                foreach (KeyValuePair<LycanthropeTotemDef, int> usedTotem in CompLycanthrope.usedTotemTracker)
                {
                    tooltip += usedTotem.Key.StatBonusList(CompLycanthrope, true);
                }

                return tooltip;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CompExposeData()
        {
            Scribe_Values.Look(ref currentStress, "currentStress", 0);
            Scribe_Values.Look(ref stressMax, "stressMax", 0);
            Scribe_Collections.Look(ref linkedHediffs, "linkedHediffs", lookMode: LookMode.Reference);
        }
    }
}
