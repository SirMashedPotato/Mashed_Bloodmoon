using RimWorld;
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
            
            if (targetPawn.Corpse == null)
            {
                ThoughtUtility.GiveThoughtsForPawnExecuted(targetPawn, parent.pawn, PawnExecutionKind.GenericBrutal);
            }
            
            if (LycanthropeUtility.PawnHasWolfsbaneHediff(targetPawn))
            {
                LycanthropeUtility.LycanthropeIngestedWolfsbane(parent.pawn);
            }

            if (LycanthropeUtility.PawnIsLycanthrope(targetPawn))
            {
                LycanthropeUtility.TransferTotems(parent.pawn, targetPawn);
                targetPawn.health.RemoveHediff(targetPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_Lycanthrope));
            }

            LycanthropeUtility.ProgressBeastHunts(parent.pawn, targetPawn, BeastHuntType.Heart);

            LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.UseTotem(parent.pawn, 1);
            CompLycanthropeTransformed.StressMax += 1;
            parent.pawn.records.Increment(RecordDefOf.Mashed_Bloodmoon_HeartsConsumed);
            
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
            return pawn.health.hediffSet.GetNotMissingParts(tag: BodyPartTagDefOf.BloodPumpingSource).RandomElement();
        }
    }
}
