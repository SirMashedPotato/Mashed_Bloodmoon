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
            if (!compLycanthrope.usedTotemTracker.ContainsKey(Props.totemTypeDef))
            {
                compLycanthrope.usedTotemTracker.Add(Props.totemTypeDef, 0);
            }
            compLycanthrope.usedTotemTracker[Props.totemTypeDef]++;
        }

        public override AcceptanceReport CanBeUsedBy(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            if (compLycanthrope == null)
            {
                return "Mashed_Bloodmoon_NotLycanthrope".Translate(pawn);
            }

            if (compLycanthrope.usedTotemTracker.TryGetValue(Props.totemTypeDef, out int usedCount) && usedCount >= Props.totemTypeDef.useLimit)
            {
                return "Mashed_Bloodmoon_TotemLimitReached".Translate(pawn, Props.totemTypeDef);
            }

            return true;
        }
    }
}
