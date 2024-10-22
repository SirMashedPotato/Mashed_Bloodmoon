using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    internal static class LycanthropeUtility
    {
        internal static readonly float lycanthropeStressToTicks = GenDate.TicksPerHour * 0.3f;
        internal static readonly int lycanthropeStressRate = GenDate.TicksPerHour / 10;

        /// <summary>
        /// Returns the pawns HediffComp_Lycanthrope
        /// </summary>
        internal static HediffComp_Lycanthrope GetCompLycanthrope(Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_Lycanthrope).TryGetComp<HediffComp_Lycanthrope>();
        }

        /// <summary>
        /// Adds the transformation fagtigue hediff
        /// </summary>
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

        internal static void ForceTransformation(Pawn pawn, Hediff dormantHediff)
        {
            pawn.health.RemoveHediff(dormantHediff);
            pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_Lycanthrope);
            HediffComp_LycanthropeTransformed compTransformed = pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformed).TryGetComp<HediffComp_LycanthropeTransformed>();
            compTransformed.StartFury();
        }

        internal static void AddLinkedHediff(Pawn pawn, HediffDef hediffDef, BodyPartDef partDef)
        {
            foreach (BodyPartRecord bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts())
            {
                if (bodyPartRecord.def == partDef)
                {
                    Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn, bodyPartRecord);
                    pawn.health.AddHediff(hediff);
                }
            }
        }

        internal static void RemoveLinkedHediff(Pawn pawn, HediffDef hediffDef)
        {
            while (true) 
            {
                Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);
                if(hediff == null)
                {
                    return;
                }
                pawn.health.RemoveHediff(hediff);
            }
        }
    }
}
