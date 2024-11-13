using RimWorld;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_ConsumeHeart : CompAbilityEffect
    {
        public new CompProperties_ConsumeHeart Props => (CompProperties_ConsumeHeart)props;

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
            
            LycanthropeUtility.UseTotem(parent.pawn, TotemTypeDefOf.Mashed_Bloodmoon_ConsumedHearts, 1);
            HediffComp_LycanthropeTransformed comp_LycanthropeTransformed = LycanthropeUtility.GetCompLycanthropeTransformed(parent.pawn);
            comp_LycanthropeTransformed.StressMax += 1;
            parent.pawn.records.Increment(RecordDefOf.Mashed_Bloodmoon_HeartsConsumed);
            
            float maxHealth = partRecord.def.GetMaxHealth(targetPawn);
            DamageInfo dinfo = new DamageInfo(DamageDefOf.Bite, maxHealth, 1, -1, parent.pawn, partRecord);
            targetPawn.health.AddHediff(RimWorld.HediffDefOf.MissingBodyPart, partRecord, dinfo);
            
            parent.pawn.needs.food.CurLevel += (Props.nutritionFactor * maxHealth);
            targetPawn.Corpse.Ingested(parent.pawn, Props.nutritionFactor * maxHealth);
            
            base.Apply(target, dest);
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (!LycanthropeUtility.PawnIsLycanthrope(parent.pawn))
            {
                reason = "Mashed_Bloodmoon_NotLycanthrope".Translate(parent.pawn);
                return true;
            }

            if (!parent.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Eating))
            {
                reason = "AbilityDisabledNoCapacity".Translate(parent.pawn, PawnCapacityDefOf.Eating.label);
                return true;
            }

            return base.GizmoDisabled(out reason);
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
            if (pawn.health.hediffSet.hediffs.Where((Hediff x) => x.Part == part).Any())
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
            ///return pawn.health.hediffSet.GetBodyPartRecord(RimWorld.BodyPartDefOf.Heart);
            return pawn.health.hediffSet.GetNotMissingParts(tag: BodyPartTagDefOf.BloodPumpingSource).RandomElement();
        }
    }
}
