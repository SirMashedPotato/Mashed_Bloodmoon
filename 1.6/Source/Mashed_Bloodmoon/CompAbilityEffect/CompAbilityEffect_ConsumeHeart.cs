using RimWorld;
using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_ConsumeHeart : LycanthropeAbilityEffectComp
    {
        public new CompProperties_AbilityConsumeHeart Props => (CompProperties_AbilityConsumeHeart)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Pawn targetPawn = ConsumeHeartUtility.GetTargetPawn(target.Thing);
            BodyPartRecord partRecord = ConsumeHeartUtility.GetBodyPartRecord(targetPawn);

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
                LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.UseTotem(parent.pawn, consumedHeartCount, Mashed_Bloodmoon_ModSettings.Lycanthropy_EnableConsumeHeartMessage);
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

                parent.pawn.Map.designationManager.TryRemoveDesignationOn(target.Thing, DesignationDefOf.Mashed_Bloodmoon_ConsumeHeart);
            }
            catch
            {
                Log.Warning(parent.pawn + " tried to consume heart of " + target.Thing + " but it was already missing");
            }
            
            base.Apply(target, dest);
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (target.Thing == null)
            {
                return false;
            }

            if (!parent.pawn.CanReserve(target.Thing))
            {
                if (throwMessages)
                {
                    Messages.Message("CannotUseReserved".Translate(), target.Thing, MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }

            return ConsumeHeartUtility.IsvalidThing(target.Thing) && base.Valid(target, throwMessages);
        }
    }
}
