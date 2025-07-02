using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffComp_Lycanthrope : HediffComp
    {
        public HediffCompProperties_Lycanthrope Props => (HediffCompProperties_Lycanthrope)props;

        private LycanthropeBeastFormDef beastFormDef;
        private LycanthropeTransformationTypeDef transformationTypeDef;
        public DefMap<WorkTypeDef, int> cachedWorkPriorities;
        public Color primaryColour = Color.white;
        public Color secondaryColour = Color.white;
        public Color tertiaryColour = Color.white;

        public HashSet<LycanthropeAbilityDef> unlockedAbilityTracker = new HashSet<LycanthropeAbilityDef>();
        public LycanthropeClawTypeDef equippedClawType;
        public HashSet<LycanthropeClawTypeDef> unlockedClawTracker = new HashSet<LycanthropeClawTypeDef>();
        // The int is the level of the totem
        public Dictionary<LycanthropeTotemDef, int> usedTotemTracker = new Dictionary<LycanthropeTotemDef, int>();
        // The int is the progress towards the beast hunt
        public Dictionary<LycanthropeBeastHuntDef, int> beastHuntTracker = new Dictionary<LycanthropeBeastHuntDef, int>();
        public int completedBeastHunts = 0;

        List<FloatMenuOption> lycanthropeGizmoOptions;

        public LycanthropeBeastFormDef BeastFormDef
        {
            get
            {
                return beastFormDef;
            }
            set
            {
                beastFormDef = value;
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
                beastFormDef = otherLycanthrope.BeastFormDef;
                transformationTypeDef = otherLycanthrope.TransformationTypeDef;
                cachedWorkPriorities = otherLycanthrope.cachedWorkPriorities;
                primaryColour = otherLycanthrope.primaryColour;
                secondaryColour = otherLycanthrope.secondaryColour;
                tertiaryColour = otherLycanthrope.tertiaryColour;
                unlockedAbilityTracker = otherLycanthrope.unlockedAbilityTracker;
                equippedClawType = otherLycanthrope.equippedClawType;
                unlockedClawTracker = otherLycanthrope.unlockedClawTracker;
                usedTotemTracker = otherLycanthrope.usedTotemTracker;
                beastHuntTracker = otherLycanthrope.beastHuntTracker;
                completedBeastHunts = otherLycanthrope.completedBeastHunts;
            }
        }

        public void ResetColours()
        {
            primaryColour = beastFormDef.PrimaryColorDefault;
            secondaryColour = beastFormDef.SecondaryColorDefault;
            tertiaryColour = beastFormDef.TertiaryColorDefault;
        }

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

        // Caches the pawns work priorities before the transformed hediff is added
        // The transformed hediff disables all work types, and resets work priorities when the transformation ends
        private void CacheWorkPriorities()
        {
            cachedWorkPriorities = new DefMap<WorkTypeDef, int>();
            foreach (WorkTypeDef workTypeDef in DefDatabase<WorkTypeDef>.AllDefs)
            {
                cachedWorkPriorities[workTypeDef] = parent.pawn.workSettings.GetPriority(workTypeDef);
            }
        }

        public override void CompPostMake()
        {
            base.CompPostMake();
            beastFormDef = LycanthropeTypeDefOf.Mashed_Bloodmoon_Werewolf;
            transformationTypeDef = LycanthropeTransformationTypeDefOf.Mashed_Bloodmoon_Bloodmoon;
            ResetColours();
            usedTotemTracker.Add(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0);
            unlockedAbilityTracker.Add(LycanthropeAbilityDefOf.Mashed_Bloodmoon_ConsumeHeart);
            equippedClawType = LycanthropeClawTypeDefOf.Mashed_Bloodmoon_LycanthropeClaws;
            unlockedClawTracker.Add(LycanthropeClawTypeDefOf.Mashed_Bloodmoon_LycanthropeClaws);
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);
            if (ModsConfig.IdeologyActive)
            {
                Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.Mashed_Bloodmoon_LycanthropeDied));
            }
        }

        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            if (Pawn.story.traits.HasTrait(TraitDefOf.Mashed_Bloodmoon_UncontrollableLycanthropy) && TransformationUtility.PawnCanTransform(Pawn, true) && Rand.Chance(0.6f))
            {
                TransformPawn(true);
            }
            else
            {
                if (TransformationUtility.PawnCanTransform(parent.pawn))
                {
                    bool continueFlag = true;
                    if (parent.pawn.Faction != null)
                    {
                        if (parent.pawn.Faction.IsPlayer)
                        {
                            if (parent.pawn.IsPrisonerOfColony)
                            {
                                if (!Mashed_Bloodmoon_ModSettings.Lycanthropy_PrisonersTransformOnDamage)
                                {
                                    continueFlag = false;
                                }
                            }
                            else if (ModsConfig.IdeologyActive && parent.pawn.IsSlaveOfColony && !Mashed_Bloodmoon_ModSettings.Lycanthropy_SlavesTransformOnDamage)
                            {
                                continueFlag = false;
                            }
                        }
                        else if (!parent.pawn.Faction.HostileTo(Faction.OfPlayer))
                        {
                            continueFlag = false;
                        }
                    }

                    if (continueFlag)
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
            }

            base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
        }

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

                bool continueFlag = true;

                if (parent.pawn.IsPrisonerOfColony)
                {
                    if (Mashed_Bloodmoon_ModSettings.Lycanthropy_PrisonersHideGizmo)
                    {
                        continueFlag = false;
                    }
                }
                else if (ModsConfig.IdeologyActive && parent.pawn.IsSlaveOfColony && Mashed_Bloodmoon_ModSettings.Lycanthropy_SlavesHideGizmo)
                {
                    continueFlag = false;
                }

                if (continueFlag)
                {
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
        }

        public override string CompDescriptionExtra
        {
            get
            {
                string description = "\n";

                ///Consumed hearts
                description += "\n" + LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.LabelCap + ": "
                    + usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts];

                description += "\n - " + LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts.StatBonusLine(this);

                return description;
            }
        }

        public override void CompExposeData()
        {
            Scribe_Deep.Look(ref cachedWorkPriorities, "cachedWorkPriorities");
            Scribe_Defs.Look(ref equippedClawType, "equippedClawType");
            Scribe_Defs.Look(ref beastFormDef, "beastFormDef");
            Scribe_Defs.Look(ref transformationTypeDef, "transformationTypeDef");
            Scribe_Values.Look(ref primaryColour, "primaryColour", Color.white);
            Scribe_Values.Look(ref secondaryColour, "secondaryColour", Color.white);
            Scribe_Values.Look(ref tertiaryColour, "tertiaryColour", Color.white);
            Scribe_Collections.Look(ref unlockedAbilityTracker, "unlockedAbilityTracker", LookMode.Def);
            Scribe_Collections.Look(ref unlockedClawTracker, "unlockedClawTracker", LookMode.Def);
            Scribe_Collections.Look(ref usedTotemTracker, "usedTotemTracker", LookMode.Def);
            Scribe_Collections.Look(ref beastHuntTracker, "beastHuntTracker", LookMode.Def);
            Scribe_Values.Look(ref completedBeastHunts, "completedBeastHunts", 0);
        }
    }
}
