using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class DamageWorker_CutSilver : DamageWorker_Cut
    {
        public override DamageResult Apply(DamageInfo dinfo, Thing thing)
        {

            float pawnSilverWeakness = thing.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropeSilverWeakness);
            if (pawnSilverWeakness > 0)
            {
                float silverDamageFactor = dinfo.Weapon.GetStatValueAbstract(StatDefOf.Mashed_Bloodmoon_SilverDamageFactor);
                dinfo.SetAmount(LycanthropeUtility.LycanthropeSilverDamageFactor(dinfo.Amount, pawnSilverWeakness, silverDamageFactor > 0 ? silverDamageFactor : 1f));
            }

            return base.Apply(dinfo, thing);
        }
    }
}
