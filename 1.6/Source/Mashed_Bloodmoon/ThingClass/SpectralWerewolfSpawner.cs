using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Mashed_Bloodmoon
{
    public class SpectralWerewolfSpawner : ThingWithComps
    {
        public int werewolfLifespan = 100;
        protected int secondarySpawnTick;
        private Effecter effecter;
        private static readonly IntRange DefaultSpawnDelay = new IntRange(600, 900);

        public int TicksUntilSpawn => secondarySpawnTick - Find.TickManager.TicksGame;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (!respawningAfterLoad)
            {
                secondarySpawnTick = Find.TickManager.TicksGame + DefaultSpawnDelay.RandomInRange;
            }
            CreateFX();
        }

        protected override void TickInterval(int delta)
        {
            if (!Spawned)
            {
                return;
            }
            effecter?.EffectTick(this, this);
            if (secondarySpawnTick <= Find.TickManager.TicksGame)
            {
                Map map = Map;
                IntVec3 position = Position;
                effecter?.Cleanup();
                allowDestroyNonDestroyable = true;
                Destroy();
                allowDestroyNonDestroyable = false;
                Spawn(map, position);
            }
            base.TickInterval(delta);
        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
        }

        private void CreateFX()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                effecter = def.building?.groundSpawnerSustainedEffecter?.Spawn(this, Map);
            });
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref secondarySpawnTick, "secondarySpawnTick", 0);
            Scribe_Values.Look(ref werewolfLifespan, "werewolfLifespan", 100);
            base.ExposeData();
        }

        protected void Spawn(Map map, IntVec3 loc)
        {
            Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Mashed_Bloodmoon_SpectralWerewolf, null);
            GenSpawn.Spawn(pawn, loc, map);
            AddHediff(pawn);
            SoundDefOf.Mashed_Bloodmoon_BeastHowl.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map));
            MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mashed_Bloodmoon_TransformEffect, Vector3.zero, pawn.DrawSize.y);
            pawn.mindState.mentalStateHandler.TryStartMentalState(RimWorld.MentalStateDefOf.ManhunterPermanent);
        }

        private void AddHediff(Pawn pawn)
        {
            Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Mashed_Bloodmoon_SpectralBeast, pawn);
            HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
            if (hediffComp_Disappears != null)
            {
                hediffComp_Disappears.ticksToDisappear = werewolfLifespan;
            }
            pawn.health.AddHediff(hediff);
        }
    }
}
