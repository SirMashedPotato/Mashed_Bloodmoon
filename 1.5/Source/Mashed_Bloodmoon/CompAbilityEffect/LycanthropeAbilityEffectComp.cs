using RimWorld;

namespace Mashed_Bloodmoon
{
    public abstract class LycanthropeAbilityEffectComp : CompAbilityEffect
    {
        public HediffComp_Lycanthrope compLycanthrope;
        public HediffComp_LycanthropeTransformed compLycanthropeTransformed;

        public HediffComp_Lycanthrope CompLycanthrope
        {
            get
            {
                if (compLycanthrope == null)
                {
                    compLycanthrope = LycanthropeUtility.GetCompLycanthrope(parent.pawn);
                }
                return compLycanthrope;
            }
        }

        public HediffComp_LycanthropeTransformed CompLycanthropeTransformed
        {
            get
            {
                if (compLycanthropeTransformed == null)
                {
                    compLycanthropeTransformed = LycanthropeUtility.GetCompLycanthropeTransformed(parent.pawn);
                }
                return compLycanthropeTransformed;
            }
        }
    }
}
