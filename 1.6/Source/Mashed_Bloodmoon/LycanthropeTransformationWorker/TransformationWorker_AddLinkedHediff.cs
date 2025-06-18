using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class TransformationWorker_AddLinkedHediff : LycanthropeTransformationWorker
    {
        public override void PostTransformationBegin(Pawn pawn, int value = 0)
        {
            if (partDef != null)
            {
                LycanthropeUtility.GetCompLycanthropeTransformed(pawn).AddLinkedHediff(hediffDef, partDef);
                return;
            }

            LycanthropeUtility.GetCompLycanthropeTransformed(pawn).AddLinkedHediff(hediffDef);
        }

        public override void PostTransformationEnd(Pawn pawn, int value = 0)
        {
            return;
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (hediffDef == null)
            {
                yield return "hediffDef is null";
            }
        }

        public HediffDef hediffDef;
        public BodyPartDef partDef;
    }
}
