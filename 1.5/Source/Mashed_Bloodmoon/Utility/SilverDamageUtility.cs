using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    internal static class SilverDamageUtility
    {
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
    }
}
