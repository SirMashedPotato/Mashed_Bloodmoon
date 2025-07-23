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
        private int? stressGain;
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
            set 
            { 
                stressMax = value;
            }
        }

        public int StressGain
        {
            get
            {
                if (stressGain == null)
                {
                    stressGain = (int)parent.pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropicStressGain);
                }
                return stressGain.Value;
            }
            set
            {
                stressGain = value;
            }
        }

        public int CurrentFatigueDuration() => (int)(currentStress * LycanthropeUtility.lycanthropeStressToTicks);

        public float CurrentTransformedDuration() => currentStress * LycanthropeUtility.lycanthropeStressRate;

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            base.CompPostTickInterval(ref severityAdjustment, delta);
            try
            {
                base.CompPostTick(ref severityAdjustment);
                if (parent.pawn.IsHashIntervalTick(LycanthropeUtility.lycanthropeStressRate, delta))
                {
                    if (currentStress >= StressMax)
                    {
                        if (inFury)
                        {
                            parent.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
                            return;
                        }

                        if (parent.pawn.Downed || parent.pawn.Deathresting)
                        {
                            parent.pawn.health.RemoveHediff(parent);
                            return;
                        }

                        StartFury();
                    }
                    else
                    {
                        if (inFury)
                        {
                            currentStress += StressGain <= 0 ? 2 : StressGain * 2;
                        }
                        else
                        {
                            currentStress = Mathf.Clamp(currentStress + StressGain, 0, StressMax);
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

        public override void CompPostMake()
        {
            base.CompPostMake();
            currentStress = 0;

            linkedHediffs = new List<Hediff> { };
            AddLinkedHediff(CompLycanthrope.equippedClawType.clawHediffDef, RimWorld.BodyPartDefOf.Hand);
            AddLinkedHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeFangs, BodyPartDefOf.Jaw);

            TransformationUtility.MoveEquippedToInventory(parent.pawn);
        }

        // Transformation workers are called here instead of in CompPostMake due to potential null references when trying to fetch this comp
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);

            parent.pawn.jobs.StopAll();
            CompLycanthrope.TransformationTypeDef.PostTransformationBegin(parent.pawn);

            CompLycanthrope.BeastFormDef.PostTransformationBegin(parent.pawn);
            foreach (LycanthropeAbilityDef unlockedAbility in CompLycanthrope.unlockedAbilityTracker)
            {
                parent.pawn.abilities.GainAbility(unlockedAbility.abilityDef);
            }
            foreach (KeyValuePair<LycanthropeTotemDef, int> usedTotem in CompLycanthrope.usedTotemTracker)
            {
                usedTotem.Key.PostTransformationBegin(parent.pawn, usedTotem.Value);
            }
            foreach (KeyValuePair<LycanthropeBeastHuntDef, int> beastHuntDef in CompLycanthrope.beastHuntTracker)
            {
                if (beastHuntDef.Key.Completed(beastHuntDef.Value))
                {
                    beastHuntDef.Key.PostTransformationBegin(parent.pawn);
                }
            }
        }

        public void AddLinkedHediff(Hediff hediff)
        {
            linkedHediffs.Add(hediff);
        }

        public void AddLinkedHediff(HediffDef hediffDef, BodyPartRecord bodyPartRecord = null)
        {
            Hediff hediff = HediffMaker.MakeHediff(hediffDef, parent.pawn, bodyPartRecord);
            parent.pawn.health.AddHediff(hediff);
            AddLinkedHediff(hediff);
        }

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

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();

            parent.pawn.jobs.StopAll();
            CompLycanthrope.TransformationTypeDef.PostTransformationEnd(parent.pawn);

            RemoveLinkedHediffs();
            CompLycanthrope.BeastFormDef.PostTransformationEnd(parent.pawn);
            foreach (LycanthropeAbilityDef unlockedAbility in CompLycanthrope.unlockedAbilityTracker)
            {
                parent.pawn.abilities.RemoveAbility(unlockedAbility.abilityDef);
            }
            foreach (KeyValuePair<LycanthropeTotemDef, int> usedTotem in CompLycanthrope.usedTotemTracker)
            {
                usedTotem.Key.PostTransformationEnd(parent.pawn, usedTotem.Value);
            }
            foreach (KeyValuePair<LycanthropeBeastHuntDef, int> beastHuntDef in CompLycanthrope.beastHuntTracker)
            {
                if (beastHuntDef.Key.Completed(beastHuntDef.Value))
                {
                    beastHuntDef.Key.PostTransformationEnd(parent.pawn);
                }
            }

            int fatigueDuration = CurrentFatigueDuration();
            if (inFury)
            {
                fatigueDuration *= 2;
            }
            TransformationUtility.AddFatigueHediff(parent.pawn, fatigueDuration);
            parent.pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformationEnd);
        }

        public void RemoveLinkedHediffs()
        {
            foreach (Hediff hediff in linkedHediffs)
            {
                hediff?.pawn.health.RemoveHediff(hediff);
            }
            linkedHediffs.Clear();
        }

        // Doing this here instead of using HediffCompProperties_DisappearsOnDeath avoids an out of bounds error if the pawn dies while transformed
        public override void Notify_PawnKilled()
        {
            base.Notify_PawnKilled();
            parent.pawn.health.RemoveHediff(parent);
        }

        public override void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
        {
            if (victim != null)
            {
                LycanthropeUtility.ProgressBeastHunts(parent.pawn, victim, BeastHuntType.Kill);
                if (Rand.Chance(parent.pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropeKillStressReductionChance)))
                {
                    currentStress -= Mathf.Clamp((int)(StressMax * 0.9f), 0, currentStress);
                    MoteMaker.MakeAttachedOverlay(parent.pawn, ThingDefOf.Mashed_Bloodmoon_TransformEffect, Vector3.zero, parent.pawn.DrawSize.y);
                    Messages.Message("Mashed_Bloodmoon_StressReduced".Translate(parent.pawn), parent.pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
            base.Notify_KilledPawn(victim, dinfo);
        }

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            bool continueFlag = true;

            if (parent.pawn.IsPrisonerOfColony)
            {
                if (Mashed_Bloodmoon_ModSettings.Lycanthropy_PrisonersHideGizmo)
                {
                    continueFlag = false;
                }
            }
            else if (ModsConfig.IdeologyActive && parent.pawn.IsSlaveOfColony && Mashed_Bloodmoon_ModSettings.Lycanthropy_SlavesHideGizmo)
            {
                continueFlag = false;
            }

            if (continueFlag)
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
        }

        public override string CompDescriptionExtra
        {
            get
            {
                string tooltip = "\n";

                foreach (KeyValuePair<LycanthropeTotemDef, int> usedTotem in CompLycanthrope.usedTotemTracker)
                {
                    tooltip += "\n - " + usedTotem.Key.StatBonusLine(CompLycanthrope, true);
                }

                return tooltip;
            }
        }

        public override void CompExposeData()
        {
            Scribe_Values.Look(ref currentStress, "currentStress", 0);
            Scribe_Values.Look(ref stressMax, "stressMax", 0);
            Scribe_Collections.Look(ref linkedHediffs, "linkedHediffs", lookMode: LookMode.Reference);
        }
    }
}
