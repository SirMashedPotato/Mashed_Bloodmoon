using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Mashed_Bloodmoon
{
    public class CompUsable_WolfsbloodAltar : CompUsable
    {
        public new CompProperties_UsableAltar Props => (CompProperties_UsableAltar)props;

        public override void UsedBy(Pawn p)
        {
            if (Props.compUseEffects) 
            {
                base.UsedBy(p);
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (Props.showUseGizmo)
            {
                yield return new Command_Action
                {
                    icon = ContentFinder<Texture2D>.Get(Props.gizmoTexPath, true),
                    defaultLabel = Props.useLabel,
                    defaultDesc = Props.gizmoDescription,
                    action = delegate
                    {
                        RimWorld.SoundDefOf.Tick_Tiny.PlayOneShotOnCamera();
                        Find.Targeter.BeginTargeting(this);
                    }
                };
            }
        }

        public override AcceptanceReport CanBeUsedBy(Pawn p, bool forced = false, bool ignoreReserveAndReachable = false)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(p);
            Building_WolfsbloodAltar altar = parent as Building_WolfsbloodAltar;
            if (Props.onlyLycanthrope && compLycanthrope == null)
            {
                return "Mashed_Bloodmoon_NotLycanthrope".Translate(p);
            }

            if (Props.onlyHuman && compLycanthrope != null)
            {
                return "Mashed_Bloodmoon_IsLycanthrope".Translate(p);
            }

            if (!Props.allowTransformed && LycanthropeUtility.PawnIsTransformedLycanthrope(p))
            {
                return "Mashed_Bloodmoon_InBeastForm".Translate(p);
            }

            if (Props.heartCost > 0 && compLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts] - Props.heartCost < 0)
            {
                return "Mashed_Bloodmoon_AbilityNotEnoughHearts".Translate(parent);
            }

            if (Props.bloodCost > 0 && !altar.CanConsumeBlood(Props.bloodCost))
            {
                return "Mashed_Bloodmoon_WolfsbloodAltarEmpty".Translate(altar);
            }

            return base.CanBeUsedBy(p, forced, ignoreReserveAndReachable);
        }
    }
}
