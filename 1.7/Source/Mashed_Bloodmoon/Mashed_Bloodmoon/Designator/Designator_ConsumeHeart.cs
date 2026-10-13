using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Designator_ConsumeHeart : Designator
    {
        private readonly List<Thing> justDesignated = new List<Thing>();

        protected override DesignationDef Designation => DesignationDefOf.Mashed_Bloodmoon_ConsumeHeart;

        public override DrawStyleCategoryDef DrawStyleCategory => DrawStyleCategoryDefOf.FilledRectangle;

        public override bool Visible => Mashed_Bloodmoon_ModSettings.EnableConsumeHeartOrder;

        public Designator_ConsumeHeart()
        {
            defaultLabel = "Mashed_Bloodmoon_DesignatorConsumeHeart_Label".Translate();
            icon = ContentFinder<Texture2D>.Get("UI/Abilities/Mashed_Bloodmoon_ConsumeHeart");
            defaultDesc = "Mashed_Bloodmoon_DesignatorConsumeHeart_Desc".Translate();
            soundDragSustain = RimWorld.SoundDefOf.Designate_DragStandard;
            soundDragChanged = RimWorld.SoundDefOf.Designate_DragStandard_Changed;
            useMouseIcon = true;
            soundSucceeded = RimWorld.SoundDefOf.Designate_Hunt;
        }

        public override AcceptanceReport CanDesignateCell(IntVec3 loc)
        {
            if (!loc.InBounds(Map))
            {
                return false;
            }
            if (ValidTargetsInCell(loc).EnumerableNullOrEmpty())
            {
                return "Mashed_Bloodmoon_DesignatorConsumeHeart_InvalidTarget".Translate();
            }
            return true;
        }

        public override void DesignateSingleCell(IntVec3 c)
        {
            foreach (Thing thing in ValidTargetsInCell(c)) 
            { 
                DesignateThing(thing);
            }
        }

        public override AcceptanceReport CanDesignateThing(Thing t)
        {
            return ConsumeHeartUtility.IsvalidThing(t, true);
        }

        public override void DesignateThing(Thing t)
        {
            Map.designationManager.RemoveAllDesignationsOn(t);
            Map.designationManager.AddDesignation(new Designation(t, Designation));
            justDesignated.Add(t);
        }

        protected override void FinalizeDesignationSucceeded()
        {
            base.FinalizeDesignationSucceeded();
            foreach(Thing thing in justDesignated)
            {
                if (thing is Pawn p && p.Downed)
                {
                    ShowDesignationWarnings(p);
                }
            }
        }

        public static void ShowDesignationWarnings(Pawn pawn)
        {
            if (pawn.Faction != null || pawn.RaceProps.Humanlike)
            {
                Messages.Message("Mashed_Bloodmoon_DesignatorConsumeHeart_DownedWarning".Translate(), pawn, MessageTypeDefOf.CautionInput, historical: false);
            }

            SlaughterDesignatorUtility.CheckWarnAboutVeneratedAnimal(pawn);
        }

        public IEnumerable<Thing> ValidTargetsInCell(IntVec3 c)
        {
            if (c.Fogged(Map))
            {
                yield break;
            }

            List<Thing> thingList = c.GetThingList(Map);
            foreach (Thing thing in thingList)
            {
                if (CanDesignateThing(thing))
                {
                    yield return thing;
                }
            }
        }
    }
}
