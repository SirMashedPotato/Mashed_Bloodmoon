using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using static UnityEngine.Random;

namespace Mashed_Bloodmoon
{
    public class LycanthropeTotemDef : Def
    {
        public string labelShort = "???";
        [NoTranslate]
        public string iconPath = "Things/Item/Special/Mashed_Bloodmoon_Totem/Mashed_Bloodmoon_TotemHeart";
        public ThingDef totemThingDef;
        public List<StatDef> statDefs;
        public int maxLevel = 30;
        public int purchaseHeartCost = 10;
        public float statIncreasePerLevel = 1f;
        public bool onlyTransformed = true;
        public bool displayAsTotem = true;
        public bool canBeTransferred = true;
        public bool canBePurchased = true;
        public LycanthropeTransformationWorker transformationWorker;

        public string LabelShortCap => labelShort.CapitalizeFirst();

        public string IconTexPath => totemThingDef != null ? totemThingDef.graphic.path : iconPath;
        public Color IconColor => totemThingDef != null ? totemThingDef.graphic.color : Color.white;

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
        /// Utility method to check if the pawn can upgrade the totem
        /// </summary>
        public bool CanUpgrade(HediffComp_Lycanthrope compLycanthrope)
        {
            if (compLycanthrope.usedTotemTracker.TryGetValue(this, 0) >= maxLevel)
            {
                return false;
            }
            if (compLycanthrope.usedTotemTracker.TryGetValue(LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts, 0) < purchaseHeartCost)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Utility method for purchasing a totem level with hearts
        /// </summary>
        public void PurchaseTotemLevel(HediffComp_Lycanthrope compLycanthrope)
        {
            compLycanthrope.usedTotemTracker[LycanthropeTotemDefOf.Mashed_Bloodmoon_ConsumedHearts] -= purchaseHeartCost;
            UseTotem(compLycanthrope, 1);
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
            int finalCount = Mathf.Clamp(usedCount, 0, maxLevel - compLycanthrope.usedTotemTracker[this]);
            if (finalCount > 0)
            {
                compLycanthrope.usedTotemTracker[this] += finalCount;
                Messages.Message("Mashed_Bloodmoon_TotemLevelUp".Translate(compLycanthrope.parent.pawn, this, finalCount), compLycanthrope.parent.pawn, MessageTypeDefOf.PositiveEvent);
            }
        }

        /// <summary>
        /// Utility method to get a lycanthropes current totem stat bonus
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
            return TotemStatBonus(compLycanthrope, out bonus);
        }

        // <summary>
        /// Utility method to get a lycanthropes current totem stat bonus
        /// </summary>
        public bool TotemStatBonus(HediffComp_Lycanthrope compLycanthrope, out float bonus)
        {
            bonus = 0;
            if (compLycanthrope.usedTotemTracker.TryGetValue(this, out int usedCount))
            {
                bonus = usedCount * statIncreasePerLevel;
                return true;
            }
            return false;
        }

        /// <summary>
        /// A list of all currently active stat bonuses from this totem
        /// </summary>
        public string StatBonusList(HediffComp_Lycanthrope compLycanthrope, bool displayOnlyTransformed)
        {
            string tooltip = "";

            foreach (StatDef statDef in statDefs)
            {
                tooltip += "\n - " + StatBonusLine(statDef, compLycanthrope, displayOnlyTransformed);
            }

            return tooltip;
        }

        /// <summary>
        /// A singular stat bonus from this totem
        /// </summary>
        public string StatBonusLine(StatDef statDef, HediffComp_Lycanthrope compLycanthrope, bool displayOnlyTransformed)
        {
            TotemStatBonus(compLycanthrope, out float bonus);
            string tooltip =  statDef.LabelCap + ": " + bonus.ToStringWithSign("0.###");
            if (!onlyTransformed && displayOnlyTransformed)
            {
                tooltip += " " + "Mashed_Bloodmoon_TotemActiveWhileHuman".Translate();
            }

            return tooltip;
        }
    }
}
