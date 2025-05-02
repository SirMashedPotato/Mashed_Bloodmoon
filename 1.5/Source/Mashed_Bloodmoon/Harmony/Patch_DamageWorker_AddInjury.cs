using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    /// <summary>
    /// Increase damage taken if the source is made out of silver
    /// </summary>
    [HarmonyPatch(typeof(DamageWorker_AddInjury))]
    [HarmonyPatch("ApplyToPawn")]
    public static class DamageWorker_AddInjury_ApplyToPawn_Patch
    {
        public static void Prefix(DamageInfo dinfo, Pawn pawn)
        {
            float silverWeakness = pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropeSilverWeakness);
            if (silverWeakness > 0)
            {
                if (dinfo.Instigator is Pawn p)
                {
                    if (p.equipment != null && p.equipment.Primary != null)
                    {
                        Thing t = p.equipment.Primary;
                        if (t.Stuff != null && t.Stuff == RimWorld.ThingDefOf.Silver
                            || (t.def.costList != null && t.def.costList.Where(x => x.thingDef == RimWorld.ThingDefOf.Silver) != null))
                        {
                            dinfo.SetAmount(dinfo.Amount * (silverWeakness + 1f));
                        }
                    }
                }
            }
        }
    }
}
