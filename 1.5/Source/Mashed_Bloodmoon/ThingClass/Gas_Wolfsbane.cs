﻿using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Gas_Wolfsbane : Gas
    {
        private readonly int tickInterval = 60;

        public override void Tick()
        {
            base.Tick();
            try
            {
                if (this.IsHashIntervalTick(tickInterval))
                {
                    HashSet<Thing> hashSet = new HashSet<Thing>(Position.GetThingList(Map));
                    if (hashSet != null)
                    {
                        foreach (Thing thing in hashSet)
                        {
                            if (thing != null && thing is Pawn p && LycanthropeUtility.PawnIsTransformedLycanthrope(p))
                            {
                                LycanthropeUtility.LycanthropeIngestedWolfsbane(p, 0.01f);
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
