using RimWorld;
using System.Collections.Generic;
using System;
using Verse;
using static System.Net.Mime.MediaTypeNames;

namespace Mashed_Bloodmoon
{
    public class ThoughtWorker_SilverApparel : ThoughtWorker
    {

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            int wornCount = WornCount(p);
            if (wornCount > 0)
            {
                return ThoughtState.ActiveAtStage(0, wornCount.ToString());
            }
            return ThoughtState.Inactive;
        }

        public override float MoodMultiplier(Pawn p)
        {
            return WornCount(p);
        }

        public int WornCount(Pawn p)
        {
            int wornCount = 0;

            List<Apparel> wornApparel = p.apparel.WornApparel;
            for (int i = 0; i < wornApparel.Count; i++)
            {
                if (wornApparel[i].Stuff == RimWorld.ThingDefOf.Silver)
                {
                    wornCount++;
                }
            }

            return wornCount;
        }
    }
}
