﻿using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    public class TotemTypeDef : Def
    {
        public string labelShort = "???";
        public ThingDef totemThingDef;
        public List<StatDef> statDefs;
        public AbilityDef abilityDef;
        public int abilityUnlocksAt = 10;
        public int useLimit = 30;
        public float increasePerLevel = 1f;
        public bool onlyTransformed = true;
        public bool displayAsTotem = true;
        public bool canBeTransferred = true;
        public LycanthropeTypeTransformationWorker transformationWorker;

        public string LabelShortCap => labelShort.CapitalizeFirst();

        public bool AbilityUnlocked(int usedCount) => abilityDef != null && usedCount >= abilityUnlocksAt;

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
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
        /// Utility method for using a totem
        /// </summary>
        public void UseTotem(Pawn pawn, int usedCount)
        {
            UseTotem(LycanthropeUtility.GetCompLycanthrope(pawn), usedCount);
        }

        /// <summary>
        /// Utility method for using a totem def
        /// Adds the totem to the lycanthropes totem tracker if it is missing
        /// </summary>
        public void UseTotem(HediffComp_Lycanthrope compLycanthrope, int usedCount)
        {
            if (!compLycanthrope.usedTotemTracker.ContainsKey(this))
            {
                compLycanthrope.usedTotemTracker.Add(this, 0);
            }
            int finalCount = Mathf.Clamp(usedCount, 0, useLimit - compLycanthrope.usedTotemTracker[this]);
            if (finalCount > 0)
            {
                compLycanthrope.usedTotemTracker[this] += finalCount;
                Messages.Message("Mashed_Bloodmoon_TotemLevelUp".Translate(compLycanthrope.parent.pawn, this, finalCount), compLycanthrope.parent.pawn, MessageTypeDefOf.PositiveEvent);
            }
        }

        /// <summary>
        /// Utility method to get a lyncathropes current totem stat bonus
        /// </summary>
        public bool TotemStatBonus(Pawn pawn, out float bonus, bool ignoreTransformed = false)
        {
            bonus = 0;
            HediffComp_Lycanthrope compLycanthrope = LycanthropeUtility.GetCompLycanthrope(pawn);
            if (compLycanthrope == null)
            {
                return false;
            }
            if (onlyTransformed && !LycanthropeUtility.PawnIsTransformedLycanthrope(pawn) && !ignoreTransformed)
            {
                return false;
            }
            if (compLycanthrope.usedTotemTracker.TryGetValue(this, out int usedCount))
            {
                bonus = usedCount * increasePerLevel;
                return true;
            }
            return false;
        }
    }
}
