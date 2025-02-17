using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Mashed_Bloodmoon
{
    public class CompAbilityEffect_SummonBeast : LycanthropeAbilityEffectComp
    {
        public new CompProperties_AbilitySummonBeast Props => (CompProperties_AbilitySummonBeast)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            for (int i = 0; i < Props.count; i++)
            {
                Pawn pawn = PawnGenerator.GeneratePawn(Props.pawnKindDef, null);
                GenSpawn.Spawn(pawn, target.Cell, parent.pawn.MapHeld, WipeMode.Vanish);
                AddLinkedHediff(pawn);
                PlaySummonEffects(pawn);
                StartMentalState(pawn);
            }
            base.Apply(target, dest);
        }

        private void AddLinkedHediff(Pawn pawn)
        {
            Hediff hediff = HediffMaker.MakeHediff(Props.linkedHediff, pawn);
            HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
            if (hediffComp_Disappears != null)
            {
                hediffComp_Disappears.ticksToDisappear = parent.def.GetStatValueAbstract(RimWorld.StatDefOf.Ability_Duration).SecondsToTicks();
            }
            pawn.health.AddHediff(hediff);
            CompLycanthropeTransformed.linkedHediffs.Add(hediff);
        }

        private void PlaySummonEffects(Pawn pawn) 
        {
            SoundDefOf.Mashed_Bloodmoon_BeastHowl.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map));
            MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mashed_Bloodmoon_TransformEffect, Vector3.zero, pawn.DrawSize.y);
        }

        private void StartMentalState(Pawn pawn)
        {
            if (Props.stateDef != null)
            {
                pawn.mindState.mentalStateHandler.TryStartMentalState(Props.stateDef);
            }
        }
    }
}
