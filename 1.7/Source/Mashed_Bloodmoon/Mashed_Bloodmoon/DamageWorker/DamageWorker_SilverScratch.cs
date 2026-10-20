using Verse;

namespace Mashed_Bloodmoon
{
    public class DamageWorker_SilverScratch : DamageWorker_Scratch
    {
        public override DamageResult Apply(DamageInfo dinfo, Thing thing)
        {
            return base.Apply(DamageUtility.ApplySilverDamageFactor(dinfo, thing), thing);
        }
    }
}
