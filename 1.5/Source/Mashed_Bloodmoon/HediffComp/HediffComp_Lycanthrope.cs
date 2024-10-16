using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffComp_Lycanthrope : HediffComp
    {
        public HediffCompProperties_Lycanthrope Props => (HediffCompProperties_Lycanthrope)props;

        public LycanthropeTypeDef lycanthropeTypeDef;
        private List<FloatMenuOption> lycanthropeTypeOptions;

        /// <summary>
        /// Creates a list of conditions for the Force new condition dev gizmo
        /// </summary>
        private List<FloatMenuOption> LycanthropeTypeOptions
        {
            get
            {
                lycanthropeTypeOptions = new List<FloatMenuOption>();

                foreach (LycanthropeTypeDef def in DefDatabase<LycanthropeTypeDef>.AllDefs)
                {
                    FloatMenuOption item;
                    AcceptanceReport allowed = def.PawnRequirementsMet(parent.pawn);
                    if (allowed)
                    {
                        item = new FloatMenuOption(def.label, delegate
                        {
                            lycanthropeTypeDef = def;
                        });
                        lycanthropeTypeOptions.Add(item);
                    }
                    else
                    {
                        item = new FloatMenuOption(def.label + " (" + allowed.Reason + ")", delegate
                        {
                            
                        });
                        lycanthropeTypeOptions.Add(item);
                    }
                }
                return lycanthropeTypeOptions;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CompPostMake()
        {
            base.CompPostMake();
            lycanthropeTypeDef = LycanthropeTypeDefOf.Mashed_Bloodmoon_Werewolf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            if (DebugSettings.ShowDevGizmos)
            {
                yield return new Command_Action
                {
                    defaultLabel = "DEV: Swap lycanthrope type",
                    action = delegate ()
                    {
                        FloatMenu floatMenu = new FloatMenu(LycanthropeTypeOptions);
                        Find.WindowStack.Add(floatMenu);
                    },
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string CompDescriptionExtra => base.CompDescriptionExtra;

        /// <summary>
        /// 
        /// </summary>
        public override void CompExposeData()
        {
            Scribe_Defs.Look(ref lycanthropeTypeDef, "lycanthropeTypeDef");
        }
    }
}
