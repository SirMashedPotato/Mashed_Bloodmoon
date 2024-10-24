using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class StatPart_LycanthropeTotems : StatPart
    {
        public override string ExplanationPart(StatRequest req)
        {
            if (req.Thing != null && req.Thing is Pawn && (!totemTypeDef.onlyTransformed || LycanthropeUtility.PawnIsTransformedLycanthrope(req.Pawn)))
            {
                return "Mashed_Bloodmoon_TotemStatOffset".Translate();
            }
            return null;
        }

        public override void TransformValue(StatRequest req, ref float val)
        {
            Log.Message("1");
            if (req.Thing == null)
            {
                return;
            }
            if (req.Thing is Pawn pawn)
            {
                Log.Message("2");
                if (totemTypeDef == null)
                {
                    return;
                }
                Log.Message("3");
                HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
                if (compLycanthrope == null)
                {
                    return;
                }
                Log.Message("4");
                if (totemTypeDef.onlyTransformed && !LycanthropeUtility.PawnIsTransformedLycanthrope(pawn))
                {
                    return;
                }
                Log.Message("5");
                if (compLycanthrope.usedTotemTracker.TryGetValue(totemTypeDef, out int usedCount))
                {
                    val += usedCount * totemTypeDef.increasePerLevel;
                }
                Log.Message("6");
            }
            
        }

        public TotemTypeDef totemTypeDef;
    }
}
