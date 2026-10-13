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

                int levelsGained = CompLycanthrope.GainExperience(targetPawn, GetExperienceGain(targetPawn));
                if (levelsGained > 0)
                {
                    CompLycanthropeTransformed.StressMax += levelsGained;
                }

                float maxHealth = partRecord.def.GetMaxHealth(targetPawn);
                DamageInfo dinfo = new DamageInfo(DamageDefOf.Bite, maxHealth, 1, -1, parent.pawn, partRecord);
                targetPawn.health.AddHediff(RimWorld.HediffDefOf.MissingBodyPart, partRecord, dinfo);
                
                GetSatiationGain(maxHealth, out float foodGain, out float restGain, out float psyfocusGain);
                parent.pawn.needs?.food?.CurLevel += foodGain;
                parent.pawn.needs?.rest?.CurLevel += restGain;
                if (ModsConfig.RoyaltyActive && parent.pawn.psychicEntropy != null)
                {
                    parent.pawn.psychicEntropy.OffsetPsyfocusDirectly(psyfocusGain);
                }

                parent.pawn.Map.designationManager.TryRemoveDesignationOn(target.Thing, DesignationDefOf.Mashed_Bloodmoon_ConsumeHeart);
            }
            catch
            {
                Log.Warning(parent.pawn + " tried to consume heart of " + target.Thing + " but it was already missing");
            }
            
            base.Apply(target, dest);
        }

        public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
        {
            if (!ConsumeHeartUtility.IsvalidThing(target.Thing))
            {
                return base.ExtraLabelMouseAttachment(target);
            }
            
            Pawn targetPawn = ConsumeHeartUtility.GetTargetPawn(target.Thing);

            return "Mashed_Bloodmoon_ConsumeHeartExtraAttachment".Translate(GetExperienceGain(targetPawn));
        }

        private void GetSatiationGain(float maxHealth, out float foodGain, out float restGain, out float psyfocusGain)
        {
            float satiationFactor = parent.pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropeHeartSatiationFactor);

            foodGain = satiationFactor * maxHealth;
            restGain = satiationFactor / 2f * maxHealth;
            psyfocusGain = satiationFactor / 3f * maxHealth;
        }

        private int GetExperienceGain(Pawn targetPawn)
        {
            if (targetPawn.kindDef == null || targetPawn.kindDef.combatPower <= 0)
            {
                return 0;
            }
            return (int)targetPawn.kindDef.combatPower * Mashed_Bloodmoon_ModSettings.Cheat_ConsumedHeartMultiplier; //TODO add exp gain stat?
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
