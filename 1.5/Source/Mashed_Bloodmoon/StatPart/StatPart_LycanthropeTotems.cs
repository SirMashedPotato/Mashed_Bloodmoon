using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class StatPart_LycanthropeTotems : StatPart
    {
        public override string ExplanationPart(StatRequest req)
        {
            if (req.Thing != null && req.Thing is Pawn pawn && totemTypeDef.TotemStatBonus(pawn, out float bonus))
            {
                return "Mashed_Bloodmoon_TotemStatOffset".Translate(totemTypeDef.LabelCap) + bonus.ToStringByStyle(parentStat.toStringStyle);
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
                if (totemTypeDef.TotemStatBonus(pawn, out float bonus))
                {
                    if (isFactor)
                    {
                        val *= (parentStat.defaultBaseValue + bonus);
                    }
                    else
                    {
                        val += bonus;
                    }
                }
            }
        }
        public LycanthropeTotemDef totemTypeDef;
        public bool isFactor = false;
    }
}
