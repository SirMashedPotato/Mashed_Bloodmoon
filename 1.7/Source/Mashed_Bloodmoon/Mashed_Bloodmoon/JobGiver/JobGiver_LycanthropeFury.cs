using RimWorld;
using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class JobGiver_LycanthropeFury : ThinkNode_JobGiver
    {
        private const int MinMeleeChaseTicks = 840;
        private const int MaxMeleeChaseTicks = 1800;
        private float maxAttackDistance = 40f;

        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.TryGetAttackVerb(null) == null)
            {
                return null;
            }
            Thing thing = FindAttackTargetPawn(pawn);
            Job job;
            if (thing != null)
            {
                job = JobMaker.MakeJob(JobDefOf.AttackMelee, thing);
                job.maxNumMeleeAttacks = 1;
                job.expiryInterval = Rand.Range(MinMeleeChaseTicks, MaxMeleeChaseTicks);
                job.canBashDoors = true;
                return job;
            }
            thing = FindAttackTargetBuilding(pawn);
            if (thing != null)
            {
                job = JobMaker.MakeJob(JobDefOf.AttackMelee, thing);
                job.expiryInterval = Rand.Range(MinMeleeChaseTicks, MaxMeleeChaseTicks);
                job.canBashDoors = true;
                return job;
            }
            return null;
        }

        private Thing FindAttackTargetPawn(Pawn pawn)
        {
            return (Thing)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedReachable, IsGoodTarget, 0f, maxAttackDistance, default, float.MaxValue, canBashDoors: true);
        }

        private Thing FindAttackTargetBuilding(Pawn pawn)
        {
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.Touch, TraverseParms.For(pawn), maxAttackDistance);
        }

        protected virtual bool IsGoodTarget(Thing thing)
        {
            if (thing is Pawn pawn && pawn.Spawned && !pawn.Downed)
            {
                return !pawn.IsPsychologicallyInvisible();
            }
            return false;
        }

        public override ThinkNode DeepCopy(bool resolve = true)
        {
            JobGiver_LycanthropeFury obj = (JobGiver_LycanthropeFury)base.DeepCopy(resolve);
            obj.maxAttackDistance = maxAttackDistance;
            return obj;
        }
    }
}