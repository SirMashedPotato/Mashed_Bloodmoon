using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class TransformationWorker_Explosion : LycanthropeTransformationWorker
    {
        public override void PostTransformationBegin(Pawn pawn, int value = 0)
        {
            GenExplosion.DoExplosion(pawn.Position, pawn.Map, radius, explosionDamageDef, null, ignoredThings: new List<Thing> { pawn });
        }

        public override void PostTransformationEnd(Pawn pawn, int value = 0)
        {
            return;
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (explosionDamageDef == null)
            {
                yield return "explosionDamageDef is null";
            }
        }

        public DamageDef explosionDamageDef;
        public float radius = 1.9f;
    }
}
