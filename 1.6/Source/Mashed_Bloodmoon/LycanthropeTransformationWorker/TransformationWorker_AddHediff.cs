using System;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    [Obsolete]
    public class TransformationWorker_AddHediff : LycanthropeTransformationWorker
    {
        public override void PostTransformationBegin(Pawn pawn, int value = 0)
        {
            if (hediffDef != null)
            {
                pawn.health.AddHediff(hediffDef);
            }
        }

        public override void PostTransformationEnd(Pawn pawn, int value = 0)
        {
            if (hediffDef != null)
            {
                Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);
                if (hediff != null) 
                { 
                    pawn.health.RemoveHediff(hediff); 
                }
            }
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (hediffDef == null)
            {
                yield return "hediffDef is null";
            }
        }

        public HediffDef hediffDef;
    }
}
