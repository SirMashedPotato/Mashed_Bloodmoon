using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_AnimalBuff : CompAbilityEffect_AddLinkedHediff
    {
        public new CompProperties_AbilityAnimalBuff Props => (CompProperties_AbilityAnimalBuff)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.Pawn == null || !target.Pawn.NonHumanlikeOrWildMan())
            {
                return;
            }

            if ((target.Pawn.Faction != null && target.Pawn.Faction == Faction.OfPlayerSilentFail)
                || (target.Pawn.InMentalState && target.Pawn.MentalState.def == MentalStateDefOf.Mashed_Bloodmoon_SpectralBeast))
            {
                base.Apply(target, dest);

                if (Props.beastHuntDef != null)
                {
                    if (Props.beastHuntDef.Completed(parent.pawn))
                    {
                        Hediff hediff = target.Pawn.health.hediffSet.GetFirstHediffOfDef(Props.hediffDef);
                        if (hediff != null)
                        {
                            hediff.Severity = Props.proficientSeverity;
                        }
                    }
                    else
                    {
                        Props.beastHuntDef.ProgressBeastHunt(parent.pawn);
                    }
                }
            }
        }
    }
}
