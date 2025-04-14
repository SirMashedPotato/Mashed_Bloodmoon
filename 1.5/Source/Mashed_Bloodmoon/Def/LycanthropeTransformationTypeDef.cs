using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;


namespace Mashed_Bloodmoon
{
    public class LycanthropeTransformationTypeDef : Def
    {
        public ThingDef moteDef;
        public SoundDef soundDef;
        public LycanthropeTypeRequirementWorker requirementWorker;
        //Only used for PostTransformationBegin
        public LycanthropeTransformationWorker transformationWorker;

        public void PlayTransformationEffects(Pawn pawn)
        {
            if (moteDef != null)
            {
                MoteMaker.MakeAttachedOverlay(pawn, moteDef, Vector3.zero, pawn.DrawSize.y);

            }
            if (soundDef != null)
            {
                soundDef.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map));
            }

            transformationWorker?.PostTransformationBegin(pawn);
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
