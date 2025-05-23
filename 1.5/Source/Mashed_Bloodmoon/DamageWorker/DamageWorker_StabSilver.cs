﻿using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    /// <summary>
    /// Should only be used for ranged weapons, melee weapons should already be covered by Verb_MeleeAttackDamage_DamageInfosToApply_Patch
    /// </summary>
    public class DamageWorker_StabSilver : DamageWorker_Stab
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
