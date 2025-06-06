using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompUseEffect_Totem : CompUseEffect
    {
        public CompProperties_UseEffectTotem Props => (CompProperties_UseEffectTotem)props;

        public override void PrepareTick()
        {
        }

        public override void DoEffect(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            Props.totemTypeDef.UseTotem(compLycanthrope, Props.usedCount);
        }

        public override AcceptanceReport CanBeUsedBy(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            if (compLycanthrope == null)
            {
                return "Mashed_Bloodmoon_NotLycanthrope".Translate(pawn);
            }

            if (!Props.totemTypeDef.CanUpgrade(compLycanthrope))
            {
                return "Mashed_Bloodmoon_TotemLimitReached".Translate(pawn, Props.totemTypeDef);
            }

            return true;
        }
    }
}
