using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class JobGiver_AICastConsumeHeart : JobGiver_AICastAbility
    {
        public IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            foreach (Designation item in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Mashed_Bloodmoon_ConsumeHeart))
            {
                if (pawn.CanReserve(item.target))
                {
                    yield return item.target.Thing;
                }
            }
        }

        protected override LocalTargetInfo GetTarget(Pawn caster, Ability ability)
        {
            if (!Mashed_Bloodmoon_ModSettings.EnableConsumeHeartOrder)
            {
                return LocalTargetInfo.Invalid;
            }

            IEnumerable<Thing> potentionalTargets = PotentialWorkThingsGlobal(caster);
            if (potentionalTargets.EnumerableNullOrEmpty())
            {
                return LocalTargetInfo.Invalid;
            }

            return GenClosest.ClosestThing_Global_Reachable(caster.Position, caster.Map, potentionalTargets, PathEndMode.Touch, TraverseParms.For(caster));
        }
    }
}
