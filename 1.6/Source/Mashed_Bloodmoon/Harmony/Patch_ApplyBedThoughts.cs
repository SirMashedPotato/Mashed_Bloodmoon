using HarmonyLib;
using RimWorld;
using Verse;

namespace Mashed_Bloodmoon
{
    /// <summary>
    /// Gives lycanthropes the restless sleep thought
    /// </summary>
    [HarmonyPatch(typeof(Toils_LayDown))]
    [HarmonyPatch("ApplyBedThoughts")]
    public static class Toils_LayDown_ApplyBedThoughts_Patch
    {
        public static void Postfix(Pawn actor, Building_Bed bed)
        {
            if (LycanthropeUtility.PawnIsLycanthrope(actor))
            {
                if (actor.needs.mood == null)
                {
                    return;
                }

                if (bed != null && bed == actor.ownership.OwnedBed && !bed.ForPrisoners)
                {
                    int index = 0;

                    if (bed.GetRoom().Role == RoomRoleDefOf.Bedroom)
                    {
                        index = 1;
                    }
                    else if (bed.GetRoom().Role == RoomRoleDefOf.Barracks)
                    {
                        index = 2;
                    }
                    actor.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.Mashed_Bloodmoon_RestlessSleep, index));
                }
            }
        }
    }
}
