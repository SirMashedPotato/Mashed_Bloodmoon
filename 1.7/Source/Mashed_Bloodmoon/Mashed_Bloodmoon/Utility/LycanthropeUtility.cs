using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    [StaticConstructorOnStartup]
    internal static class LycanthropeUtility
    {
        internal const int experiencePerLevel = 100;
        internal const int stressTicksPerHour = 10;
        internal const float lycanthropeStressToTicks = GenDate.TicksPerHour * 0.3f;
        internal const int lycanthropeStressRate = GenDate.TicksPerHour / stressTicksPerHour;
        private const float totemTransferPercent = 0.3f;
        private const int baseLevelCap = 99;
        public static int levelCapIncreasePerBeastHunt;
        
        static LycanthropeUtility()
        {
            int beastHuntCount = DefDatabase<LycanthropeBeastHuntDef>.AllDefsListForReading.Count;
            int maxLevelCap = LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.maxLevel;
            levelCapIncreasePerBeastHunt = (maxLevelCap - baseLevelCap) / beastHuntCount;
        }
        
        internal static int GetCurrentLevelCap(int beastHuntsCompleted = 0) => levelCapIncreasePerBeastHunt * beastHuntsCompleted + baseLevelCap;
        
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
            return GetLycanthropeHediff(pawn)?.TryGetComp<HediffComp_Lycanthrope>();
        }

        internal static Hediff GetLycanthropeHediff(Pawn pawn)
        {
            return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_Lycanthrope);
        }

        internal static HediffComp_LycanthropeTransformed GetCompLycanthropeTransformed(Pawn pawn)
        {
            return GetTransformedHediff(pawn)?.TryGetComp<HediffComp_LycanthropeTransformed>();
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
                Dictionary<LycanthropeTotemDef, int> gainedTotems = new Dictionary<LycanthropeTotemDef, int>();
                foreach (KeyValuePair<LycanthropeTotemDef, int> usedTotem in victimCompLycanthrope.usedTotemTracker)
                {
                    if (usedTotem.Key.canBeTransferred)
                    {
                        int count = (int)(usedTotem.Value * totemTransferPercent);
                        transferredTotemCount += count;
                        gainedTotems.Add(usedTotem.Key, count);
                    }
                }

                if (transferredTotemCount > 0)
                {
                    foreach (KeyValuePair<LycanthropeTotemDef, int> gainedTotem in gainedTotems)
                    {
                        gainedTotem.Key.UseTotem(pawn, gainedTotem.Value, false);
                    }
                    Messages.Message("Mashed_Bloodmoon_ConsumedTotemsTransferred".Translate(pawn, transferredTotemCount, victim), pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
        }

        internal static void ProgressBeastHunts(Pawn parent, Pawn victim, BeastHuntType beastHuntType)
        {
            List<LycanthropeBeastHuntDef> beastHuntList = DefDatabase<LycanthropeBeastHuntDef>.AllDefsListForReading.Where(x => x.beastHuntType == beastHuntType
                && ((x.targetThingDef == null && x.targetKindDef == null && x.targetXenotypeDef == null) 
                || x.targetThingDef == victim.def 
                || x.targetKindDef == victim.kindDef
                || (victim.genes != null && x.targetXenotypeDef == victim.genes.Xenotype))).ToList();
            if (beastHuntList.NullOrEmpty())
            {
                return;
            }
            foreach (LycanthropeBeastHuntDef beastHuntDef in beastHuntList)
            {
                beastHuntDef.ProgressBeastHunt(parent);
            }
        }
        
        internal static int GetExperienceGain(Pawn parent, float baseAmount)
        {

            return (int)(baseAmount * parent.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropeExperienceGainFactor) * Mashed_Bloodmoon_ModSettings.Cheat_ConsumedHeartMultiplier);
        }
    }
}
