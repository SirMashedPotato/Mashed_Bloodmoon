﻿using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class GreatBeastDef : Def
    {
        public ThingDef thingDef;
        public int consumeCount = 1;
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

        public bool Completed(int currentCount)
        {
            return currentCount >= consumeCount;
        }

        /// <summary>
        /// Utility method for consuming a great beast heart
        /// </summary>
        public void ConsumeGreatBeastHeart(Pawn pawn)
        {
            ConsumeGreatBeastHeart(LycanthropeUtility.GetCompLycanthrope(pawn), pawn);
        }

        /// <summary>
        /// Utility method for consuming a great beast heart
        /// </summary>
        public void ConsumeGreatBeastHeart(HediffComp_Lycanthrope compLycanthrope, Pawn pawn)
        {
            if (!compLycanthrope.greatBeastHeartTracker.ContainsKey(this))
            {
                compLycanthrope.greatBeastHeartTracker.Add(this, 0);
            }

            if (Completed(compLycanthrope.greatBeastHeartTracker[this]))
            {
                return;
            }

            compLycanthrope.greatBeastHeartTracker[this]++;

            if (Completed(compLycanthrope.greatBeastHeartTracker[this]))
            {
                Messages.Message("Mashed_Bloodmoon_GreatBeastHuntComplete".Translate(pawn, this), pawn, MessageTypeDefOf.PositiveEvent);
            }
        }
    }
}
