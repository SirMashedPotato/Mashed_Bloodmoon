using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LycanthropeAbilityDef : LycanthropeDef
    {
        [NoTranslate]
        public string backgroundTexPath = "UI/Widgets/AbilityButBG";
        public AbilityDef abilityDef;

        public override bool AlreadyUnlocked(HediffComp_Lycanthrope compLycanthrope)
        {
            return compLycanthrope.unlockedAbilityTracker.Contains(this);
        }

        public override void Purchase(HediffComp_Lycanthrope compLycanthrope)
        {
            base.Purchase(compLycanthrope);
            Unlock(compLycanthrope);
        }

        public override void Unlock(HediffComp_Lycanthrope compLycanthrope)
        {
            compLycanthrope.unlockedAbilityTracker.Add(this);
            Messages.Message("Mashed_Bloodmoon_AbilityUnlocked".Translate(compLycanthrope.parent.pawn, this), compLycanthrope.parent.pawn, MessageTypeDefOf.PositiveEvent);
        }

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }

            if (abilityDef == null)
            {
                yield return "abilityDef is null";
            }
        }
    }
}
