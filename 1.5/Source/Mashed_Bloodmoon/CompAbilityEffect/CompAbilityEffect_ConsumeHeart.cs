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

            DamageInfo dinfo = new DamageInfo(DamageDefOf.Bite, partRecord.def.GetMaxHealth(targetPawn), 1, -1, parent.pawn, partRecord);
            targetPawn.health.AddHediff(RimWorld.HediffDefOf.MissingBodyPart, partRecord, dinfo);

            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(parent.pawn);
            compLycanthrope.usedTotemTracker[TotemTypeDefOf.Mashed_Bloodmoon_ConsumedHearts]++;
            parent.pawn.needs.food.CurLevel += 0.1f;

            base.Apply(target, dest);
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (!LycanthropeUtility.PawnIsLycanthrope(parent.pawn))
            {
                reason = "Mashed_Bloodmoon_NotLycanthrope".Translate(parent.pawn);
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
                    pawn = c.InnerPawn;
                }
            }

            return pawn;
        }

        private BodyPartRecord GetBodyPartRecord(Pawn pawn) 
        {
            return pawn.health.hediffSet.GetBodyPartRecord(RimWorld.BodyPartDefOf.Heart);
        }
    }
}
