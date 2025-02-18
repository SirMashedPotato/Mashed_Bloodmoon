using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffComp_KillOnRemove : HediffComp
    {
        public HediffCompProperties_KillOnRemove Props => (HediffCompProperties_KillOnRemove)props;

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            if (!parent.pawn.Dead)
            {
                parent.pawn.Kill(null);
            }
        }
    }
}
