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

        /// <summary>
        /// We don't want it doing anything other than starting the job
        /// </summary>
        public override void UsedBy(Pawn p)
        {
            return;
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
            if (!Props.allowTransformed && LycanthropeUtility.PawnIsTransformedLycanthrope(p))
            {
                return "Mashed_Bloodmoon_LycanthropeCantDo".Translate(p);
            }

            return base.CanBeUsedBy(p, forced, ignoreReserveAndReachable);
        }
    }
}
