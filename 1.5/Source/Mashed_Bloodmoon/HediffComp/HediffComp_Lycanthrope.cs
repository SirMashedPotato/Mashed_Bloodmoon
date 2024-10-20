﻿using RimWorld;
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
        /// 
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
        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            if (!parent.pawn.health.hediffSet.HasHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformed))
            {
                yield return new Command_Action
                {
                    defaultLabel = "Mashed_Bloodmoon_TransformLycanthrope_Label".Translate(),
                    defaultDesc = "Mashed_Bloodmoon_TransformLycanthrope_Desc".Translate(parent.pawn, 
                    ((int)parent.pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropicStressMax) * LycanthropeUtility.lycanthropeStressRate).ToStringTicksToPeriod()),
                    action = delegate ()
                    {
                        parent.pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformed);
                    },
                    Disabled = parent.pawn.health.hediffSet.HasHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeFatigue),
                };
            }

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
