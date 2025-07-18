﻿using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_ConsumeHeart : LycanthropeAbilityEffectComp
    {
        public new CompProperties_AbilityConsumeHeart Props => (CompProperties_AbilityConsumeHeart)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Pawn targetPawn = GetTargetPawn(target.Thing);
            BodyPartRecord partRecord = GetBodyPartRecord(targetPawn);

            if (partRecord == null)
            {
                return;
            }

            try
            {
                if (targetPawn.Corpse == null)
                {
                    ThoughtUtility.GiveThoughtsForPawnExecuted(targetPawn, parent.pawn, PawnExecutionKind.GenericBrutal);
                }

                if (DamageUtility.PawnHasWolfsbaneHediff(targetPawn))
                {
                    DamageUtility.LycanthropeIngestedWolfsbane(parent.pawn);
                }

                if (LycanthropeUtility.PawnIsLycanthrope(targetPawn))
                {
                    LycanthropeUtility.TransferTotems(parent.pawn, targetPawn);
                    targetPawn.health.RemoveHediff(targetPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_Lycanthrope));
                }

                LycanthropeUtility.ProgressBeastHunts(parent.pawn, targetPawn, BeastHuntType.Heart);

                int consumedHeartCount = 1 * Mashed_Bloodmoon_ModSettings.Lycanthropy_ConsumedHearMultiplier;
                LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.UseTotem(parent.pawn, consumedHeartCount);
                CompLycanthropeTransformed.StressMax += consumedHeartCount;

                float maxHealth = partRecord.def.GetMaxHealth(targetPawn);
                DamageInfo dinfo = new DamageInfo(DamageDefOf.Bite, maxHealth, 1, -1, parent.pawn, partRecord);
                targetPawn.health.AddHediff(RimWorld.HediffDefOf.MissingBodyPart, partRecord, dinfo);


                float nutritionFactor = parent.pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropeHeartSatiationFactor);

                if (parent.pawn.needs?.food != null)
                {
                    parent.pawn.needs.food.CurLevel += (nutritionFactor * maxHealth);
                }
                if (parent.pawn.needs?.rest != null)
                {
                    parent.pawn.needs.rest.CurLevel += ((nutritionFactor / 2f) * maxHealth);
                }
                if (ModsConfig.RoyaltyActive && parent.pawn.psychicEntropy != null)
                {
                    parent.pawn.psychicEntropy.OffsetPsyfocusDirectly(nutritionFactor / 3f * maxHealth);
                }
            }
            catch
            {

            }
            
            base.Apply(target, dest);
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (target.Thing == null)
            {
                return false;
            }

            Pawn pawn = GetTargetPawn(target.Thing);

            if (pawn == null)
            {
                return false;
            }

            BodyPartRecord part = GetBodyPartRecord(pawn);
            if (part == null)
            {
                return false;
            }
            return base.Valid(target, throwMessages);
        }

        private Pawn GetTargetPawn(Thing target)
        {
            Pawn pawn = null;
            if (target is Pawn p)
            {
                if (!p.DeadOrDowned)
                {
                    return null;
                }
                pawn = p;
            }
            else
            {
                if (target is Corpse c)
                {
                    if (c.GetRotStage() != RotStage.Fresh)
                    {
                        return null;
                    }
                    pawn = c.InnerPawn;
                }
            }

            return pawn;
        }

        private BodyPartRecord GetBodyPartRecord(Pawn pawn) 
        {
            List<BodyPartRecord> parts = pawn.health.hediffSet.GetNotMissingParts(tag: BodyPartTagDefOf.BloodPumpingSource).ToList();
            if (parts.NullOrEmpty())
            {
                return null;
            }
            return parts.RandomElement();
        }
    }
}
