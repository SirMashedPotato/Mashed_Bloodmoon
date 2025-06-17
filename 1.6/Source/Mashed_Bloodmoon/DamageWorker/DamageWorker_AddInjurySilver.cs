using Verse;

namespace Mashed_Bloodmoon
{
    public class DamageWorker_AddInjurySilver : DamageWorker_AddInjury
    {
        public override DamageResult Apply(DamageInfo dinfo, Thing thing)
        {
            return base.Apply(SilverDamageUtility.ApplySilverDamageFactor(dinfo, thing), thing);
        }
    }
}
