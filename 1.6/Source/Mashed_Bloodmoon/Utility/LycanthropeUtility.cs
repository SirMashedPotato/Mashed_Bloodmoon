using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    internal static class LycanthropeUtility
    {
        internal const float lycanthropeStressToTicks = GenDate.TicksPerHour * 0.3f;
        internal const int lycanthropeStressRate = GenDate.TicksPerHour / 10;
        internal const float totemTransferPercent = 0.3f;

        internal static bool IsNight(Pawn pawn)
        {
            return (GenLocalDate.HourInteger(pawn) >= 23 || GenLocalDate.HourInteger(pawn) <= 5);
        }
        internal static bool PawnIsDormantLycanthrope(Pawn pawn)
        {
            return pawn?.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeDormant) != null;
        }

        internal static bool PawnIsLycanthrope(Pawn pawn)
        {
            return GetLycanthropeHediff(pawn) != null;
        }

        internal static bool PawnIsTransformedLycanthrope(Pawn pawn, bool includeDummy = false)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformed) != null
                || (includeDummy && pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformedDummy) != null);
        }

        internal static HediffComp_Lycanthrope GetCompLycanthrope(Pawn pawn)
        {
            return GetLycanthropeHediff(pawn).TryGetComp<HediffComp_Lycanthrope>();
        }

        internal static Hediff GetLycanthropeHediff(Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_Lycanthrope);
        }

        internal static HediffComp_LycanthropeTransformed GetCompLycanthropeTransformed(Pawn pawn)
        {
            return GetTransformedHediff(pawn).TryGetComp<HediffComp_LycanthropeTransformed>();
        }

        internal static Hediff GetTransformedHediff(Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformed);
        }

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

        internal static void ProgressBeastHunts(Pawn parent, Pawn victim, BeastHuntType beastHuntType)
        {
            List<LycanthropeBeastHuntDef> beastHuntList = DefDatabase<LycanthropeBeastHuntDef>.AllDefsListForReading.Where(x => x.beastHuntType == beastHuntType
                && ((x.targetThingDef == null && x.targetKindDef == null) 
                || x.targetThingDef == victim.def 
                || x.targetKindDef == victim.kindDef)).ToList();
            if (beastHuntList.NullOrEmpty())
            {
                return;
            }
            foreach (LycanthropeBeastHuntDef beastHuntDef in beastHuntList)
            {
                beastHuntDef.ProgressBeastHunt(parent);
            }
        }
    }
}
