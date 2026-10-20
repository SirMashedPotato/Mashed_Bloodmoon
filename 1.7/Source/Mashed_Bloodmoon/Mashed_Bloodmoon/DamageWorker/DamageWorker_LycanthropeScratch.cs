using Verse;

namespace Mashed_Bloodmoon
{
    public class DamageWorker_LycanthropeScratch : DamageWorker_Scratch
    {
        protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageResult result)
        {
            base.ApplySpecialEffectsToPart(pawn, totalDamage, dinfo, result);
            DamageUtility.ApplyLycanthropeDamage(pawn, 0.25f);
        }
    }
}
