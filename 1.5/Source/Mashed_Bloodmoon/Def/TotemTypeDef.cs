﻿using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class TotemTypeDef : Def
    {
        public string labelShort = "???";
        public ThingDef totemThingDef;
        public List<StatDef> statDefs;
        public int useLimit = 30;
        public float increasePerLevel = 1f;
        public bool onlyTransformed = true;
        public bool displayAsTotem = true;
        public LycanthropeTypeTransformationWorker transformationWorker;

        public string LabelShortCap => labelShort.CapitalizeFirst();

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
    }
}
