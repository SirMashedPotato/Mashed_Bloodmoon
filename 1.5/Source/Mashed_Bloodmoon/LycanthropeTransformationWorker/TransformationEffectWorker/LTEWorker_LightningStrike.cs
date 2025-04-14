using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Mashed_Bloodmoon
{
    [StaticConstructorOnStartup]
    public class LTEWorker_LightningStrike : LTEWorker_Explosion
    {
        private static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt");

        public override void PostTransformationBegin(Pawn pawn, int value = 0)
        {
            Mesh boltMesh = LightningBoltMeshPool.RandomBoltMesh;
            Graphics.DrawMesh(boltMesh, pawn.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), Quaternion.identity, FadedMaterialPool.FadedVersionOf(LightningMat, 1), 0);
            RimWorld.SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(pawn.Map);
            
            base.PostTransformationBegin(pawn, value);

            SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.Position, pawn.Map));
            RimWorld.SoundDefOf.Thunder_OnMap.PlayOneShot(info);
        }

        public override void PostTransformationEnd(Pawn pawn, int value = 0)
        {
            return;
        }
    }
}
