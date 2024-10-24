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
            if (req.Thing == null)
            {
                return;
            }
            if (req.Thing is Pawn pawn)
            {
                if (LycanthropeUtility.TotemStatBonus(pawn, totemTypeDef, out float bonus))
                {
                    val += bonus;
                }
            }
        }
        public TotemTypeDef totemTypeDef;
    }
}
