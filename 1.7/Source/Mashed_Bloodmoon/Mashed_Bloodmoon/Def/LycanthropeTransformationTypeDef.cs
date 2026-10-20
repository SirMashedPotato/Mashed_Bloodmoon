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

        public override void PostTransformationBegin(Pawn pawn, int value = 0)
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

            base.PostTransformationBegin(pawn, value);
        }

        public override void PostTransformationEnd(Pawn pawn, int value = 0)
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

            base.PostTransformationEnd(pawn, value);
        }
    }
}
