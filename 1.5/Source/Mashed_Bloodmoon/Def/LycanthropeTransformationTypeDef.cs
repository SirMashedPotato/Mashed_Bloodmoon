using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;


namespace Mashed_Bloodmoon
{
    public class LycanthropeTransformationTypeDef : Def
    {
        public EffecterDef startEffecterDef;
        public EffecterDef endEffecterDef;
        public ThingDef startMoteDef;
        public ThingDef endMoteDef;
        public SoundDef startSoundDef;
        public SoundDef endSoundDef;
        public LycanthropeTypeRequirementWorker requirementWorker;
        public LycanthropeTransformationWorker transformationWorker;

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

        public AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (requirementWorker != null)
            {
                return requirementWorker.PawnRequirementsMet(pawn);
            }
            return true;
        }

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }

            if (requirementWorker != null)
            {
                foreach (string item in requirementWorker.ConfigErrors())
                {
                    yield return item;
                }
            }

            if (transformationWorker != null)
            {
                foreach (string item in transformationWorker.ConfigErrors())
                {
                    yield return item;
                }
            }
        }
    }
}
