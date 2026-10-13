using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LycanthropeClawTypeDef : LycanthropeDef
    {
        public HediffDef clawHediffDef;

        public override bool AlreadyUnlocked(HediffComp_Lycanthrope compLycanthrope)
        {
            return compLycanthrope.unlockedClawTracker.Contains(this);
        }

        public override void Purchase(HediffComp_Lycanthrope compLycanthrope)
        {
            base.Purchase(compLycanthrope);
            Unlock(compLycanthrope);
            compLycanthrope.equippedClawType = this;
        }

        public override void Unlock(HediffComp_Lycanthrope compLycanthrope)
        {
            compLycanthrope.unlockedClawTracker.Add(this);
            Messages.Message("Mashed_Bloodmoon_ClawsUnlocked".Translate(compLycanthrope.parent.pawn, this), compLycanthrope.parent.pawn, MessageTypeDefOf.PositiveEvent);
        }

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }

            if (clawHediffDef == null)
            {
                yield return "clawHediffDef is null";
            }
        }
    }
}
