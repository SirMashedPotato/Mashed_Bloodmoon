using RimWorld;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    internal static class LycanthropeUtility
    {
        internal static readonly float lycanthropeStressToTicks = GenDate.TicksPerHour * 0.3f;
        internal static readonly int lycanthropeStressRate = GenDate.TicksPerHour / 10;

        internal static bool PawnIsTransformedLycanthrope(Pawn pawn)
        {
            return pawn?.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformed) != null;
        }

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

        internal static void ApplyLycanthropeDamage(Pawn pawn, float factor = 1f)
        {
            if (pawn.RaceProps.Humanlike)
            {
                float resistance = pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropyResistance);
                if (Rand.Chance(Mathf.Max(1f - resistance, 0f) * factor))
                {
                    pawn.health.GetOrAddHediff(HediffDefOf.Mashed_Bloodmoon_SaniesLupinus).Severity += 0.01f;
                }
            }
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
            Hediff hediff;
            while ((hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef)) != null)
            {
                pawn.health.RemoveHediff(hediff);
            }
        }
    }
}
