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
        private LycanthropeTransformationTypeDef transformationTypeDef;
        public DefMap<WorkTypeDef, int> cachedWorkPriorities;
        public Color primaryColour = Color.white;
        public Color secondaryColour = Color.white;
        public Color tertiaryColour = Color.white;

        /// <summary>
        /// The int is the level of the ability
        /// Or it would be if that was implemented
        /// 1 is level 1, and would in theory correspond to the first abilityDef in the list in LycanthropeAbilityDef 
        /// </summary>
        public Dictionary<LycanthropeAbilityDef, int> unlockedAbilityTracker = new Dictionary<LycanthropeAbilityDef, int>();
        /// <summary>
        /// The int is the level of the totem
        /// </summary>
        public Dictionary<LycanthropeTotemDef, int> usedTotemTracker = new Dictionary<LycanthropeTotemDef, int>();
        /// <summary>
        /// The int is the progress towards the beast hunt
        /// </summary>
        public Dictionary<LycanthropeBeastHuntDef, int> beastHuntTracker = new Dictionary<LycanthropeBeastHuntDef, int>();
        public int completedBeastHunts = 0;

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

        public LycanthropeTransformationTypeDef TransformationTypeDef
        {
            get
            {
                return transformationTypeDef;
            }
            set
            {
                transformationTypeDef = value;
            }
        }

        public override void CopyFrom(HediffComp other)
        {
            base.CopyFrom(other);
            if (other is HediffComp_Lycanthrope otherLycanthrope)
            {
                lycanthropeTypeDef = otherLycanthrope.LycanthropeTypeDef;
                transformationTypeDef = otherLycanthrope.TransformationTypeDef;
                cachedWorkPriorities = otherLycanthrope.cachedWorkPriorities;
                primaryColour = otherLycanthrope.primaryColour;
                secondaryColour = otherLycanthrope.secondaryColour;
                tertiaryColour = otherLycanthrope.tertiaryColour;
                unlockedAbilityTracker = otherLycanthrope.unlockedAbilityTracker;
                usedTotemTracker = otherLycanthrope.usedTotemTracker;
                beastHuntTracker = otherLycanthrope.beastHuntTracker;
                completedBeastHunts = otherLycanthrope.completedBeastHunts;
            }
        }

        public void ResetColours()
        {
            primaryColour = lycanthropeTypeDef.PrimaryColorDefault;
            secondaryColour = lycanthropeTypeDef.SecondaryColorDefault;
            tertiaryColour = lycanthropeTypeDef.TertiaryColorDefault;
        }

        /// <summary>
        /// Used to activate a pawns beast form
        /// Caches work priorities before hand, because beast form (disabled work) resets them
        /// </summary>
        public void TransformPawn(bool startInFury = false)
        {
            CacheWorkPriorities();
            Hediff transformed = parent.pawn.health.AddHediff(HediffDefOf.Mashed_Bloodmoon_LycanthropeTransformed);
            if (startInFury)
            {
                transformed.TryGetComp<HediffComp_LycanthropeTransformed>().StartFury();
            }
            Hediff fatigue = parent.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Mashed_Bloodmoon_LycanthropeFatigue);
            if (fatigue != null)
            {
                parent.pawn.health.RemoveHediff(fatigue);
            }
        }

        /// <summary>
        /// Caches the pawns work priorities before the transformed hediff is added
        /// The transformed hediff disables all work types, and resets work priorities when the transformation ends
        /// </summary>
        private void CacheWorkPriorities()
        {
            cachedWorkPriorities = new DefMap<WorkTypeDef, int>();
            foreach (WorkTypeDef workTypeDef in DefDatabase<WorkTypeDef>.AllDefs)
            {
                cachedWorkPriorities[workTypeDef] = parent.pawn.workSettings.GetPriority(workTypeDef);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CompPostMake()
        {
            base.CompPostMake();
            lycanthropeTypeDef = LycanthropeTypeDefOf.Mashed_Bloodmoon_Werewolf;
            transformationTypeDef = LycanthropeTransformationTypeDefOf.Mashed_Bloodmoon_Bloodmoon;
            ResetColours();
            usedTotemTracker.Add(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0);
            unlockedAbilityTracker.Add(LycanthropeAbilityDefOf.Mashed_Bloodmoon_ConsumeHeart, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);
            if (ModsConfig.IdeologyActive)
            {
                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.Mashed_Bloodmoon_LycanthropeDied));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            if (Pawn.story.traits.HasTrait(TraitDefOf.Mashed_Bloodmoon_UncontrollableLycanthropy) && LycanthropeUtility.PawnCanTransform(Pawn, true) && Rand.Chance(0.6f))
            {
                TransformPawn(true);
            }
            else
            {
                if (LycanthropeUtility.PawnCanTransform(parent.pawn) && (parent.pawn.Faction == null || parent.pawn.Faction.HostileTo(Faction.OfPlayer)))
                {
                    float chance = 0.1f;

                    PawnLycanthropeProperties props = PawnLycanthropeProperties.GetProps(parent.pawn);
                    if (props != null)
                    {
                        chance = props.chance;
                    }

                    if (Rand.Chance(chance))
                    {
                        TransformPawn();
                    }
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

                    //Customise beast form
                    FloatMenuOption item = new FloatMenuOption("Mashed_Bloodmoon_CustomiseBeastForm".Translate(), delegate
                    {
                        Page_CustomiseBeastForm page = new Page_CustomiseBeastForm(this);
                        Find.WindowStack.Add(page);
                    });
                    lycanthropeGizmoOptions.Add(item);

                    //Upgrade beast form
                    item = new FloatMenuOption("Mashed_Bloodmoon_UpgradeBeastForm".Translate(), delegate
                    {
                        Page_UpgradeBeastForm page = new Page_UpgradeBeastForm(this);
                        Find.WindowStack.Add(page);
                    });
                    lycanthropeGizmoOptions.Add(item);

                    //Beast hunt progress
                    item = new FloatMenuOption("Mashed_Bloodmoon_BeastHuntProgress".Translate(), delegate
                    {
                        Page_BeastHuntProgress page = new Page_BeastHuntProgress(this);
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
                if (Mashed_Bloodmoon_ModSettings.Lycanthropy_EnableOptionsGizmo)
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
                }
                
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
                description += "\n" + LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.LabelCap + ": "
                    + usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts];
                foreach (StatDef statDef in LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.statDefs)
                {
                    LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.TotemStatBonus(parent.pawn, out float bonus, true);
                    description += "\n  - " + statDef.LabelCap + ": +" + bonus;
                }
                return description;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void CompExposeData()
        {
            Scribe_Deep.Look(ref cachedWorkPriorities, "cachedWorkPriorities");
            Scribe_Defs.Look(ref lycanthropeTypeDef, "lycanthropeTypeDef");
            Scribe_Defs.Look(ref transformationTypeDef, "transformationTypeDef");
            Scribe_Values.Look(ref primaryColour, "primaryColour", Color.white);
            Scribe_Values.Look(ref secondaryColour, "secondaryColour", Color.white);
            Scribe_Values.Look(ref tertiaryColour, "tertiaryColour", Color.white);
            Scribe_Collections.Look(ref unlockedAbilityTracker, "unlockedAbilityTracker", LookMode.Def);
            Scribe_Collections.Look(ref usedTotemTracker, "usedTotemTracker", LookMode.Def);
            Scribe_Collections.Look(ref beastHuntTracker, "beastHuntTracker", LookMode.Def);
            Scribe_Values.Look(ref completedBeastHunts, "completedBeastHunts", 0);
        }
    }
}
