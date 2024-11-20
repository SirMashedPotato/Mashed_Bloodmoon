﻿using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class GreatBeastDef : Def
    {
        public ThingDef thingDef;
        public LycanthropeTypeTransformationWorker transformationWorker;
        [NoTranslate]
        public string heartTexPath = "UI/Icons/Mashed_Bloodmoon_GreatBeastHeart";
        [NoTranslate]
        public string consumedTexPath = "UI/Icons/Mashed_Bloodmoon_GreatBeastHeartConsumed";

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }

            if (thingDef == null)
            {
                yield return "null thingDef";
            }

            if (transformationWorker != null)
            {
                foreach (string item in transformationWorker.ConfigErrors())
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Utility method for consuming a great beast heart
        /// </summary>
        public bool ConsumeGreatBeastHeart(Pawn pawn)
        {
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            if (!compLycanthrope.greatBeastHeartTracker.Contains(this))
            {
                compLycanthrope.greatBeastHeartTracker.Add(this);
                Messages.Message("Mashed_Bloodmoon_ConsumedGreatBeastHeart".Translate(pawn, thingDef), pawn, MessageTypeDefOf.PositiveEvent);
                return true;
            }
            return false;
        }
    }
}
