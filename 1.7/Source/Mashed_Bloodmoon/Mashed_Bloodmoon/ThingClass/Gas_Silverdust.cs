using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Gas_Silverdust : Gas
    {
        private readonly int tickInterval = 120;
        private readonly float baseDamage = 6;

        protected override void TickInterval(int delta)
        {
            base.TickInterval(delta);
            TickEffecter();
            try
            {
                if (this.IsHashIntervalTick(tickInterval, delta))
                {
                    HashSet<Thing> hashSet = new HashSet<Thing>(Position.GetThingList(Map));
                    if (hashSet != null)
                    {
                        foreach (Thing thing in hashSet)
                        {
                            if (thing != null && thing is Pawn p)
                            {
                                float silverWeakness = p.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropeSilverWeakness);
                                if (silverWeakness > 0)
                                {
                                    DamageInfo dinfo = new DamageInfo(DamageDefOf.Burn, baseDamage * silverWeakness, 0, -1, null);
                                    p.TakeDamage(dinfo);
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

        public void TickEffecter()
        {
            if (!Spawned)
            {
                Effecter temp = effecter;
                temp?.Cleanup();
                effecter = null;
                return;
            }
            if (effecter == null)
            {
                effecter = EffecterDefOf.Mashed_Bloodmoon_Silverdust.SpawnAttached(this, Map, 1f);
            }
            Effecter effecter2 = effecter;
            if (effecter2 == null)
            {
                return;
            }
            effecter2.EffectTick(this, this);
        }

        private Effecter effecter;
    }
}
