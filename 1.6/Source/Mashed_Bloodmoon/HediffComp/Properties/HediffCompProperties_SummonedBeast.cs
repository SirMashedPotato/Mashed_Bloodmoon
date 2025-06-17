using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffCompProperties_SummonedBeast : HediffCompProperties
    {
        public HediffCompProperties_SummonedBeast() => compClass = typeof(HediffComp_SummonedBeast);

        public bool killOnRemove = true;
    }
}
