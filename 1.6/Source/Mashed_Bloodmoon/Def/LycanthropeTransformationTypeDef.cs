using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;


namespace Mashed_Bloodmoon
{
    public class LycanthropeTransformationTypeDef : LycanthropeDef
    {
        public EffecterDef startEffecterDef;
        public EffecterDef endEffecterDef;
        public ThingDef startMoteDef;
        public ThingDef endMoteDef;
        public SoundDef startSoundDef;
        public SoundDef endSoundDef;

        public void PlayTransformationStartEffects(Pawn pawn)
        {

            if (startEffecterDef != null)
            {
                startEffecterDef.Spawn(pawn.Position, pawn.Map).Cleanup();
            }
            if (startMoteDef != null)
            {
                MoteMaker.MakeAttachedOverlay(pawn, startMoteDef, Vector3.zero, pawn.DrawSize.y);
            }
            if (startSoundDef != null)
            {
                startSoundDef.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map));
            }

            transformationWorker?.PostTransformationBegin(pawn);
        }

        public void PlayTransformationEndEffects(Pawn pawn)
        {

            if (endEffecterDef != null)
            {
                endEffecterDef.Spawn(pawn.Position, pawn.Map).Cleanup();
            }
            if (endMoteDef != null)
            {
                MoteMaker.MakeAttachedOverlay(pawn, endMoteDef, Vector3.zero, pawn.DrawSize.y);
            }
            if (endSoundDef != null)
            {
                endSoundDef.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map));
            }

            transformationWorker?.PostTransformationEnd(pawn);
        }
    }
}
