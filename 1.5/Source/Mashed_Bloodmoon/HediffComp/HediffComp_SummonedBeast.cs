using RimWorld;
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
            MoteMaker.MakeStaticMote(parent.pawn.Position.ToVector3(), parent.pawn.Map, ThingDefOf.Mashed_Bloodmoon_TransformEffect, parent.pawn.DrawSize.y);
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
