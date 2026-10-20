using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffComp_ForcedTransformOnRemoved : HediffComp
    {
        public HediffCompProperties_ForcedTransformOnRemoved Props => (HediffCompProperties_ForcedTransformOnRemoved)props;

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            if (!parent.pawn.Dead && parent.pawn.Spawned)
            {
                TransformationUtility.ForceTransformation(parent.pawn);
            }
        }
    }
}
