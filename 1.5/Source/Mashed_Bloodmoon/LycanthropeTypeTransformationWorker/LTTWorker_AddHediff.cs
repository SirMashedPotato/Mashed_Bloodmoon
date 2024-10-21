using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTTWorker_AddHediff : LycanthropeTypeTransformationWorker
    {
        public override void PostTransformationBegin(Pawn pawn)
        {
            if (hediffDef != null)
            {
                pawn.health.AddHediff(hediffDef);
            }
        }

        public override void PostTransformationEnd(Pawn pawn)
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
