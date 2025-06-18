using HarmonyLib;
using System;
using System.Reflection;
using Verse;

namespace Mashed_Bloodmoon
{
    /// <summary>
    /// Add any render nodes that are in the lycanthrope type def
    /// </summary>
    [HarmonyPatch(typeof(PawnRenderTree))]
    [HarmonyPatch("SetupDynamicNodes")]
    public static class PawnRenderTree_SetupDynamicNodes_Patch
    {
        public static void Postfix(PawnRenderTree __instance)
        {
            if (LycanthropeUtility.PawnIsTransformedLycanthrope(__instance.pawn, true))
            {
                LycanthropeBeastFormDef typeDef = LycanthropeUtility.GetCompLycanthrope(__instance.pawn).BeastFormDef;
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
}
