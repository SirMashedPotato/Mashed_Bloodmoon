using Verse;

namespace Mashed_Bloodmoon
{
    public class DamageWorker_SilverStab : DamageWorker_Stab
    {
        public override DamageResult Apply(DamageInfo dinfo, Thing thing)
        {
            return base.Apply(DamageUtility.ApplySilverDamageFactor(dinfo, thing), thing);
        }
    }
}
