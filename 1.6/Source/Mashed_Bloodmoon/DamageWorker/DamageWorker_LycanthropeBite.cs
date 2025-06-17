using Verse;

namespace Mashed_Bloodmoon
{
    public class DamageWorker_LycanthropeBite : DamageWorker_Bite
    {
        protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageResult result)
        {
            base.ApplySpecialEffectsToPart(pawn, totalDamage, dinfo, result);
            LycanthropeUtility.ApplyLycanthropeDamage(pawn, 0.75f);
        }
    }
}
