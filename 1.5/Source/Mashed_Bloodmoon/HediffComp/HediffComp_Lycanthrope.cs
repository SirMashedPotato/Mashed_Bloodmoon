﻿using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using static UnityEngine.Random;

namespace Mashed_Bloodmoon
{
    public class HediffComp_Lycanthrope : HediffComp
    {
        public HediffCompProperties_Lycanthrope Props => (HediffCompProperties_Lycanthrope)props;

        private LycanthropeTypeDef lycanthropeTypeDef;
        public Color primaryColour = Color.white;
        public Color secondaryColour = Color.white;
        private List<FloatMenuOption> lycanthropeTypeOptions;
        public Dictionary<TotemTypeDef, int> usedTotemTracker = new Dictionary<TotemTypeDef, int>();

        /// <summary>
        /// Debug tool, replace with proper gizmo
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

        public LycanthropeTypeDef LycanthropeTypeDef
        {
            get
            {
                return lycanthropeTypeDef;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CompPostMake()
        {
            base.CompPostMake();
            lycanthropeTypeDef = LycanthropeTypeDefOf.Mashed_Bloodmoon_Werewolf;
            primaryColour = lycanthropeTypeDef.graphicData.color;
            secondaryColour = lycanthropeTypeDef.graphicData.colorTwo;
            foreach (TotemTypeDef def in DefDatabase<TotemTypeDef>.AllDefs)
            {
                usedTotemTracker.Add(def, 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);
            Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.Mashed_Bloodmoon_LycanthropeDied));
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
                    defaultLabel = "Mashed_Bloodmoon_TransformBeast_Label".Translate(),
                    defaultDesc = "Mashed_Bloodmoon_TransformBeast_Desc".Translate(parent.pawn, 
                    ((int)parent.pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropicStressMax) * LycanthropeUtility.lycanthropeStressRate).ToStringTicksToPeriod()),
                    icon = ContentFinder<Texture2D>.Get("UI/Gizmos/Mashed_Bloodmoon_TransformBeast", true),
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
        public override string CompDescriptionExtra
        {
            get
            {
                string description = "\n";

                ///Consumed hearts
                description += "\n" + TotemTypeDefOf.Mashed_Bloodmoon_ConsumedHearts.LabelCap + ": " 
                    + usedTotemTracker[TotemTypeDefOf.Mashed_Bloodmoon_ConsumedHearts];
                foreach(StatDef statDef in TotemTypeDefOf.Mashed_Bloodmoon_ConsumedHearts.statDefs)
                {
                    LycanthropeUtility.TotemStatBonus(parent.pawn, TotemTypeDefOf.Mashed_Bloodmoon_ConsumedHearts, out float bonus, true);
                    description += "\n  - " + statDef.LabelCap + ": +" + bonus;
                }

                ///Totems
                description += "Mashed_Bloodmoon_UsedTotems".Translate();
                foreach (KeyValuePair<TotemTypeDef, int> usedTotem in usedTotemTracker)
                {
                    if (usedTotem.Key.displayAsTotem)
                    {
                        description += "\n  - " + usedTotem.Key.LabelShortCap + ": " + usedTotem.Value;
                        if (!usedTotem.Key.statDefs.NullOrEmpty())
                        {
                            foreach (StatDef statDef in usedTotem.Key.statDefs)
                            {
                                LycanthropeUtility.TotemStatBonus(parent.pawn, usedTotem.Key, out float bonus, true);
                                description += "\n    - " + statDef.LabelCap + ": ";
                                if (bonus > 0)
                                {
                                    description += "+";
                                }
                                description += bonus;
                                if (!usedTotem.Key.onlyTransformed)
                                {
                                    description += " (H)";
                                }
                            }
                            if (usedTotem.Key.AbilityUnlocked(usedTotem.Value))
                            {
                                description += "Mashed_Bloodmoon_TotemAbility".Translate(usedTotem.Key.abilityDef);
                            }
                        }
                    }
                }
                description += "Mashed_Bloodmoon_LycanthropeFormOnly".Translate();

                return description;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CompExposeData()
        {
            Scribe_Defs.Look(ref lycanthropeTypeDef, "lycanthropeTypeDef");
            Scribe_Values.Look(ref primaryColour, "primaryColour", Color.white);
            Scribe_Values.Look(ref secondaryColour, "secondaryColour", Color.white);
            Scribe_Collections.Look(ref usedTotemTracker, "usedTotemTracker", LookMode.Def);
        }
    }
}
