using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffComp_SummonedBeast : HediffComp
    {
        public HediffCompProperties_SummonedBeast Props => (HediffCompProperties_SummonedBeast)props;
        public Pawn parentPawn;


        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            if (Props.killOnRemove && !parent.pawn.Dead)
            {
                parent.pawn.Kill(null);
            }
        }

        public override string CompLabelInBracketsExtra => parentPawn.NameShortColored;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_References.Look(ref parentPawn, "parentPawn");
        }
    }
}
