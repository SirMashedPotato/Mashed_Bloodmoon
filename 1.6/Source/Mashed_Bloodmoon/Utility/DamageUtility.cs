using RimWorld;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    internal static class DamageUtility
    {
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

        internal static float LycanthropeSilverDamageFactor(float baseDamage, float pawnSilverWeakness, float silverDamageFactor)
        {
            return baseDamage * ((pawnSilverWeakness * silverDamageFactor) + 1f);
        }

        internal static DamageInfo ApplySilverDamageFactor(DamageInfo dinfo, Thing thing)
        {
            float pawnSilverWeakness = thing.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropeSilverWeakness);
            if (pawnSilverWeakness > 0)
            {
                float silverDamageFactor = dinfo.Weapon.GetStatValueAbstract(StatDefOf.Mashed_Bloodmoon_SilverDamageFactor);
                dinfo.SetAmount(LycanthropeSilverDamageFactor(dinfo.Amount, pawnSilverWeakness, silverDamageFactor > 0 ? silverDamageFactor : 1f));
            }

            return dinfo;
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
    }
}
