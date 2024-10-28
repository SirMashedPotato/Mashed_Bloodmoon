using RimWorld;
using System.Linq;
using Verse;

namespace Mashed_Bloodmoon
{
    public class HediffComp_SilverWeakness : HediffComp
    {
        public HediffCompProperties_SilverWeakness Props => (HediffCompProperties_SilverWeakness)props;

        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            if (dinfo.Def != DamageDefOf.Burn)
            {
                if (dinfo.Instigator is Pawn)
                {
                    Pawn p = dinfo.Instigator as Pawn;

                    if (p.equipment != null && p.equipment.Primary != null)
                    {
                        Thing t = p.equipment.Primary;
                        if (t.Stuff != null && t.Stuff == RimWorld.ThingDefOf.Silver 
                            || (t.def.costList != null && t.def.costList.Where(x => x.thingDef == RimWorld.ThingDefOf.Silver) != null))
                        {
                            DamageInfo silverDinfo = new DamageInfo(dinfo);
                            silverDinfo.SetAmount(totalDamageDealt * parent.pawn.GetStatValue(StatDefOf.Mashed_Bloodmoon_LycanthropeSilverWeakness));
                            silverDinfo.Def = DamageDefOf.Burn;
                            silverDinfo.SetHitPart(parent.pawn.health.hediffSet.hediffs.GetLast().Part);
                            parent.pawn.TakeDamage(silverDinfo);
                        }
                    }
                }
            }
           
            base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
        }
    }
}
