using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    internal static class LycanthropeUtility
    {
        internal static readonly float lycanthropeStressToTicks = GenDate.TicksPerHour * 0.3f;
        internal static readonly int lycanthropeStressRate = GenDate.TicksPerHour / 10;
        internal static readonly float totemTransferPercent = 0.3f;

        internal static bool IsNight(Pawn pawn)
        {
            return (GenLocalDate.HourInteger(pawn) >= 23 || GenLocalDate.HourInteger(pawn) <= 5);
        }

        internal static bool PawnCanTransform(Pawn pawn)
        {
            return !PawnIsTransformedLycanthrope(pawn) && pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeFatigue) == null;
        }

        internal static bool PawnIsLycanthrope(Pawn pawn)
        {
            return GetLycanthropeHediff(pawn) != null;
        }

        internal static bool PawnIsFatigued(Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeFatigue) != null;
        }

        internal static bool PawnIsTransformedLycanthrope(Pawn pawn, bool includeDummy = false)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformed) != null
                || (includeDummy && pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformedDummy) != null);
        }

        internal static bool PawnIsDormantLycanthrope(Pawn pawn)
        {
            return pawn?.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeDormant) != null;
        }

        internal static bool PawnHasWolfsbaneHediff(Pawn pawn)
        {
            return pawn?.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_WolfsbaneNausea) != null
                || pawn?.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_WolfsbanePrevention) != null
                || pawn?.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_WolfsbaneResistance) != null;
        }

        internal static void LycanthropeIngestedWolfsbane(Pawn pawn, float severity = 0.3f)
        {
            Hediff toxicBuildup = HediffMaker.MakeHediff(RimWorld.HediffDefOf.ToxicBuildup, pawn);
            toxicBuildup.Severity = severity * pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropeWolfsbaneWeakness);
            pawn.health.AddHediff(toxicBuildup);
        }

        /// <summary>
        /// Returns the pawns HediffComp_Lycanthrope
        /// </summary>
        internal static HediffComp_Lycanthrope GetCompLycanthrope(Pawn pawn)
        {
            return GetLycanthropeHediff(pawn).TryGetComp<HediffComp_Lycanthrope>();
        }

        /// <summary>
        /// 
        /// </summary>
        internal static Hediff GetLycanthropeHediff(Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_Lycanthrope);
        }

        /// <summary>
        /// Returns the pawns HediffComp_LycanthropeTransformed
        /// </summary>
        internal static HediffComp_LycanthropeTransformed GetCompLycanthropeTransformed(Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformed).TryGetComp<HediffComp_LycanthropeTransformed>();
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

        /// <summary>
        /// Utility method for transferring totems from one lyacnthrope to another
        /// </summary>
        internal static void TransferTotems(Pawn pawn, Pawn victim)
        {
            HediffComp_Lycanthrope victimCompLycanthrope = GetCompLycanthrope(victim);
            if (!victimCompLycanthrope.usedTotemTracker.NullOrEmpty())
            {
                int transferredTotemCount = 0;
                foreach (KeyValuePair<LycanthropeTotemDef, int> usedTotem in victimCompLycanthrope.usedTotemTracker)
                {
                    if (usedTotem.Key.canBeTransferred)
                    {
                        int count = (int)(usedTotem.Value * totemTransferPercent);
                        transferredTotemCount += count;
                        usedTotem.Key.UseTotem(pawn, count);
                    }
                }
                if (transferredTotemCount > 0)
                {
                    Messages.Message("Mashed_Bloodmoon_ConsumedTotemsTransferred".Translate(pawn, transferredTotemCount, victim), pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
        }

        /// <summary>
        /// Utility method to progress all relevant beast hunts
        /// </summary>
        internal static void ProgressBeastHunts(Pawn parent, ThingDef victim, BeastHuntType beastHuntType)
        {
            List<LycanthropeBeastHuntDef> greatBeastDefList = DefDatabase<LycanthropeBeastHuntDef>.AllDefsListForReading.Where(x => x.beastHuntType == beastHuntType
                && (x.targetThingDef == null || x.targetThingDef == victim)).ToList();
            if (greatBeastDefList.NullOrEmpty())
            {
                return;
            }
            foreach (LycanthropeBeastHuntDef greatBeastDef in greatBeastDefList)
            {
                greatBeastDef.ProgressBeastHunt(parent);
            }
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
    }
}
