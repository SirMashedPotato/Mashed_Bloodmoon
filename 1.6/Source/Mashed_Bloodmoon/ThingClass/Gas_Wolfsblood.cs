using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Gas_Wolfsblood : Gas
    {
        private readonly int tickInterval = 60;

        protected override void TickInterval(int delta)
        {
            try
            {
                if (this.IsHashIntervalTick(tickInterval, delta))
                {
                    HashSet<Thing> hashSet = new HashSet<Thing>(Position.GetThingList(Map));
                    if (hashSet != null)
                    {
                        foreach (Thing thing in hashSet)
                        {
                            if (thing != null && thing is Pawn p && LycanthropeUtility.PawnIsTransformedLycanthrope(p))
                            {
                                Hediff bloodloss = p.health.hediffSet.GetFirstHediffOfDef(RimWorld.HediffDefOf.BloodLoss);
                                if (bloodloss != null)
                                {
                                    bloodloss.Severity -= 0.01f;
                                }
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {

            }
        }
    }
}
