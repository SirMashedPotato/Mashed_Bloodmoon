using HarmonyLib;
using Verse;
using System.Reflection;
using RimWorld;
using System;
using static RimWorld.Dialog_EditIdeoStyleItems;

namespace Mashed_Bloodmoon
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            Harmony harmony = new Harmony("com.Mashed_Bloodmoon");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            harmony.Patch(AccessTools.Method(typeof(EquipmentUtility), nameof(EquipmentUtility.CanEquip), new[] { typeof(Thing), typeof(Pawn), typeof(string).MakeByRefType(), typeof(bool) }),
                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(HarmonyPatches.EquipmentUtility_CanEquip_Patch)));
        }


    }
    public static class HarmonyPatches
    {
        /// <summary>
        /// Prevents transformed lycanthropes from equipping weapons
        /// </summary>
        public static void EquipmentUtility_CanEquip_Patch(Thing thing, Pawn pawn, ref string cantReason, ref bool __result)
        {
            if (__result && LycanthropeUtility.PawnIsTransformedLycanthrope(pawn))
            {
                __result = false;
                cantReason = "Mashed_Bloodmoon_LycanthropeCantEquip".Translate(pawn);
            }
        }
    }

    /// <summary>
    /// Add any render nodes that are in the lycanthrope type def
    /// </summary>
    [HarmonyPatch(typeof(PawnRenderTree))]
    [HarmonyPatch("SetupDynamicNodes")]
    public static class PawnRenderTree_SetupDynamicNodes_Patch
    {
        public static void Postfix(PawnRenderTree __instance)
        {
            if (LycanthropeUtility.PawnIsTransformedLycanthrope(__instance.pawn))
            {
                LycanthropeTypeDef typeDef = LycanthropeUtility.GetCompLycanthrope(__instance.pawn).LycanthropeTypeDef;
                if (typeDef.RenderNodeProperties.NullOrEmpty())
                {
                    return;
                }

                MethodInfo addChild = __instance.GetType().GetMethod("AddChild", BindingFlags.NonPublic | BindingFlags.Instance);
                Hediff hediff = __instance.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformed);

                foreach (PawnRenderNodeProperties nodeProps in typeDef.RenderNodeProperties)
                {
                    PawnRenderNode pawnRenderNode = (PawnRenderNode)Activator.CreateInstance(nodeProps.nodeClass, __instance.pawn, nodeProps, __instance);
                    pawnRenderNode.hediff = hediff;
                    addChild.Invoke(__instance, new object[] { pawnRenderNode, null });
                }
            }
        }
    }

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
