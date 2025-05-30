﻿using RimWorld;
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
            int count = Props.count;

            if (Props.beastHuntDef != null && Props.beastHuntDef.Completed(CompLycanthrope))
            {
                count += Props.proficiencyExtraCount;
            }

            for (int i = 0; i < count; i++)
            {
                Pawn pawn = PawnGenerator.GeneratePawn(Props.pawnKindDef, null);
                Name pawnName = new NameSingle("Mashed_Bloodmoon_SpectralBeastName".Translate(parent.pawn.Name.ToStringShort, pawn.Label));
                pawn.Name = pawnName;
                GenSpawn.Spawn(pawn, target.Cell, parent.pawn.MapHeld);
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
            HediffComp_SummonedBeast hediffComp_SummonedBeast = hediff.TryGetComp<HediffComp_SummonedBeast>();
            if (hediffComp_SummonedBeast != null)
            {
                hediffComp_SummonedBeast.linkedPawn = parent.pawn;
                if (Props.beastHuntDef != null)
                {
                    hediffComp_SummonedBeast.beastHuntDef = Props.beastHuntDef;
                    hediffComp_SummonedBeast.incrementBeastHuntOnKill = true;
                }
            }
            pawn.health.AddHediff(hediff);
            CompLycanthropeTransformed.AddLinkedHediff(hediff);
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
                if (pawn.mindState.mentalStateHandler.TryStartMentalState(Props.stateDef))
                {
                    pawn.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks = parent.def.GetStatValueAbstract(RimWorld.StatDefOf.Ability_Duration).SecondsToTicks();
                    pawn.mindState.mentalStateHandler.CurState.sourceFaction = parent.pawn.Faction;
                }
            }
        }
    }
}
