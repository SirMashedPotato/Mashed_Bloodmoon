using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffComp_Lycanthrope : HediffComp
    {
        public HediffCompProperties_Lycanthrope Props => (HediffCompProperties_Lycanthrope)props;

        private LycanthropeTypeDef lycanthropeTypeDef;
        public Color primaryColour = Color.white;
        public Color secondaryColour = Color.white;
        public Color tertiaryColour = Color.white;

        public Dictionary<TotemTypeDef, int> usedTotemTracker = new Dictionary<TotemTypeDef, int>();
        List<FloatMenuOption> lycanthropeGizmoOptions;

        public LycanthropeTypeDef LycanthropeTypeDef
        {
            get
            {
                return lycanthropeTypeDef;
            }
            set
            {
                lycanthropeTypeDef = value;
            }
        }

        public void ResetColours()
        {
            primaryColour = lycanthropeTypeDef.PrimaryColorDefault;
            secondaryColour = lycanthropeTypeDef.SecondaryColorDefault;
            tertiaryColour = lycanthropeTypeDef.TertiaryColorDefault;
        }

        /// <summary>
        /// 
        /// </summary>
        public void TransformPawn(bool startInFury = false)
        {
            Hediff transformed = parent.pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformed);
            if (startInFury)
            {
                HediffComp_LycanthropeTransformed compTransformed = transformed.TryGetComp<HediffComp_LycanthropeTransformed>();
                compTransformed.StartFury();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CompPostMake()
        {
            base.CompPostMake();
            lycanthropeTypeDef = LycanthropeTypeDefOf.Mashed_Bloodmoon_Werewolf;
            ResetColours();
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
        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            if (LycanthropeUtility.PawnCanTransform(parent.pawn) && (parent.pawn.Faction == null || parent.pawn.Faction.HostileTo(Faction.OfPlayer)))
            {
                if (Rand.Chance(0.1f))
                {
                    TransformPawn();
                }
            }
            base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
        }

        /// <summary>
        /// 
        /// </summary>
        private List<FloatMenuOption> LycanthropeGizmoOptions
        {
            get
            {
                if (lycanthropeGizmoOptions.NullOrEmpty())
                {
                    lycanthropeGizmoOptions = new List<FloatMenuOption>();

                    ///Customise beast form
                    FloatMenuOption item = new FloatMenuOption("Mashed_Bloodmoon_CustomiseBeastForm".Translate(), delegate
                    {
                        Page_CustomiseBeastForm page = new Page_CustomiseBeastForm(this);
                        Find.WindowStack.Add(page);
                    });
                    lycanthropeGizmoOptions.Add(item);
                }
                return lycanthropeGizmoOptions;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            if (!LycanthropeUtility.PawnIsTransformedLycanthrope(parent.pawn))
            {
                yield return new Command_Action
                {
                    defaultLabel = "Mashed_Bloodmoon_LycanthropeOptions_Label".Translate(),
                    defaultDesc = "Mashed_Bloodmoon_LycanthropeOptions_Desc".Translate(parent.pawn),
                    icon = ContentFinder<Texture2D>.Get("UI/Gizmos/Mashed_Bloodmoon_LycanthropeOptions", true),
                    action = delegate ()
                    {
                        Find.WindowStack.Add(new FloatMenu(LycanthropeGizmoOptions));
                    },
                };
                yield return new Command_Action
                {
                    defaultLabel = "Mashed_Bloodmoon_TransformBeast_Label".Translate(),
                    defaultDesc = "Mashed_Bloodmoon_TransformBeast_Desc".Translate(parent.pawn,
                    ((int)parent.pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropicStressMax) * LycanthropeUtility.lycanthropeStressRate).ToStringTicksToPeriod()),
                    icon = ContentFinder<Texture2D>.Get("UI/Gizmos/Mashed_Bloodmoon_TransformBeast", true),
                    action = delegate ()
                    {
                        TransformPawn();
                    },
                    Disabled = parent.pawn.health.hediffSet.HasHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeFatigue),
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
                foreach (StatDef statDef in TotemTypeDefOf.Mashed_Bloodmoon_ConsumedHearts.statDefs)
                {
                    TotemTypeDefOf.Mashed_Bloodmoon_ConsumedHearts.TotemStatBonus(parent.pawn, out float bonus, true);
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
                                usedTotem.Key.TotemStatBonus(parent.pawn, out float bonus, true);
                                description += "\n    - " + statDef.LabelCap + ": ";
                                if (bonus > 0)
                                {
                                    description += "+";
                                }
                                description += bonus.ToString();
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
            Scribe_Values.Look(ref tertiaryColour, "tertiaryColour", Color.white);
            Scribe_Collections.Look(ref usedTotemTracker, "usedTotemTracker", LookMode.Def);
        }
    }
}
