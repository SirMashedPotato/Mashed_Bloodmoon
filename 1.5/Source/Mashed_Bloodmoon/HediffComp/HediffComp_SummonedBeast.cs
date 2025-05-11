using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffComp_SummonedBeast : HediffComp
    {
        public HediffCompProperties_SummonedBeast Props => (HediffCompProperties_SummonedBeast)props;
        public Pawn linkedPawn;
        public bool incrementBeastHuntOnKill = false;
        public LycanthropeBeastHuntDef beastHuntDef;


        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            MoteMaker.MakeStaticMote(parent.pawn.Position.ToVector3(), parent.pawn.Map, ThingDefOf.Mashed_Bloodmoon_TransformEffect, parent.pawn.DrawSize.y);
            if (Props.killOnRemove && !parent.pawn.Dead)
            {
                parent.pawn.Kill(null);
            }
        }

        public override void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
        {
            if (incrementBeastHuntOnKill && beastHuntDef != null)
            {
                beastHuntDef.ProgressBeastHunt(linkedPawn);
            }
            base.Notify_KilledPawn(victim, dinfo);
        }

        public override string CompLabelInBracketsExtra => linkedPawn.NameShortColored;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_References.Look(ref linkedPawn, "linkedPawn");
            Scribe_Values.Look(ref incrementBeastHuntOnKill, "incrementBeastHuntOnKill");
            Scribe_Defs.Look(ref beastHuntDef, "beastHuntDef");
        }
    }
}
