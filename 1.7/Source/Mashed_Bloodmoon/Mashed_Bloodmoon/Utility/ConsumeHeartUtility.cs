using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    internal static class ConsumeHeartUtility
    {
        internal static bool IsValidThing(Thing target, bool onlyCorpse = false)
        {
            Pawn pawn = GetTargetPawn(target, onlyCorpse);

            if (pawn == null)
            {
                return false;
            }

            if (!pawn.RaceProps.IsFlesh)
            {
                return false;
            }

            BodyPartRecord part = GetBodyPartRecord(pawn);
            return part != null;
        }

        internal static Pawn GetTargetPawn(Thing target, bool onlyCorpse = false)
        {
            Pawn pawn = null;
            if (!onlyCorpse && target is Pawn p)
            {
                if (!p.DeadOrDowned)
                {
                    return null;
                }
                pawn = p;
            }
            else
            {
                if (target is not Corpse c) return null;
                if (c.GetRotStage() != RotStage.Fresh)
                {
                    return null;
                }
                pawn = c.InnerPawn;
            }

            return pawn;
        }
        internal static BodyPartRecord GetBodyPartRecord(Pawn pawn)
        {
            List<BodyPartRecord> parts = pawn?.health?.hediffSet?.GetNotMissingParts(tag: BodyPartTagDefOf.BloodPumpingSource).ToList();
            return parts.NullOrEmpty() ? null : parts.RandomElement();
        }
    }
}
