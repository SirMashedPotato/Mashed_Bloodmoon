using Verse;

namespace Mashed_Bloodmoon
{
    internal static class TransformationUtility
    {
        internal static bool PawnCanTransform(Pawn pawn, bool ignoreFatigue = false)
        {
            return !LycanthropeUtility.PawnIsTransformedLycanthrope(pawn) && (ignoreFatigue || !PawnIsFatigued(pawn));
        }

        internal static bool PawnIsFatigued(Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeFatigue) != null;
        }

        internal static void ForceTransformation(Pawn pawn)
        {
            Hediff dormantHediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeDormant);
            ForceTransformation(pawn, dormantHediff);
        }

        internal static void ForceTransformation(Pawn pawn, Hediff dormantHediff)
        {
            pawn.health.RemoveHediff(dormantHediff);
            HediffComp_Lycanthrope compLycanthrope = pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_Lycanthrope).TryGetComp<HediffComp_Lycanthrope>();
            compLycanthrope.TransformPawn(true);
        }

        internal static void ApplyImminentTransformation(Pawn pawn, int baseTicks)
        {
            Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeImminentTransformation, pawn);
            HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
            if (hediffComp_Disappears != null)
            {
                hediffComp_Disappears.ticksToDisappear = (int)(baseTicks * Rand.Range(1f, 1.5f));
            }
            pawn.health.AddHediff(hediff);
        }

        internal static void MoveEquippedToInventory(Pawn pawn)
        {
            if (pawn.equipment.HasAnything())
            {
                foreach (Thing thing in pawn.equipment.GetDirectlyHeldThings())
                {
                    pawn.equipment.TryTransferEquipmentToContainer(thing as ThingWithComps, pawn.inventory.innerContainer);
                }
            }
        }

        internal static void AddFatigueHediff(Pawn pawn, int duration)
        {
            Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeFatigue, pawn);
            HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
            if (hediffComp_Disappears != null)
            {
                hediffComp_Disappears.ticksToDisappear = duration;
            }
            pawn.health.AddHediff(hediff);
        }
    }
}
