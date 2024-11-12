using RimWorld;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    internal static class LycanthropeUtility
    {
        internal static readonly float lycanthropeStressToTicks = GenDate.TicksPerHour * 0.3f;
        internal static readonly int lycanthropeStressRate = GenDate.TicksPerHour / 10;

        internal static bool PawnCanTransform(Pawn pawn)
        {
            return !PawnIsTransformedLycanthrope(pawn) && pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeFatigue) == null;
        }

        internal static bool PawnIsLycanthrope(Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_Lycanthrope) != null;
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
            return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_Lycanthrope).TryGetComp<HediffComp_Lycanthrope>();
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

        public static bool TotemStatBonus(Pawn pawn, TotemTypeDef totemTypeDef, out float bonus, bool ignoreTransformed = false)
        {
            bonus = 0;
            if (totemTypeDef == null)
            {
                return false;
            }
            HediffComp_Lycanthrope compLycanthrope = GetCompLycanthrope(pawn);
            if (compLycanthrope == null)
            {
                return false;
            }
            if (totemTypeDef.onlyTransformed && !PawnIsTransformedLycanthrope(pawn) && !ignoreTransformed)
            {
                return false;
            }
            if (compLycanthrope.usedTotemTracker.TryGetValue(totemTypeDef, out int usedCount))
            {
                bonus = usedCount * totemTypeDef.increasePerLevel;
                return true;
            }
            return false;
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
            HediffComp_Lycanthrope compLycanthrope = pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_Lycanthrope).TryGetComp<HediffComp_Lycanthrope>();
            compLycanthrope.TransformPawn(true);
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

        internal static void MoveEquippedToInventory(Pawn pawn)
        {
            if (pawn.equipment.HasAnything())
            {
                foreach(Thing thing in pawn.equipment.GetDirectlyHeldThings())
                {
                    pawn.equipment.TryTransferEquipmentToContainer(thing as ThingWithComps, pawn.inventory.innerContainer);
                }
            }
        }
    }
}
