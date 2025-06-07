using Verse;

namespace Mashed_Bloodmoon
{
    public class Building_WolfsbloodAltar : Building
    {
        private const float maxBloodAmount = 1f;
        private const float defaultChangeAmount = 0.1f;
        private float lycanthropeBloodAmount = 0;

        public bool IsFull() => lycanthropeBloodAmount >= maxBloodAmount;
        public bool IsEmpty() => lycanthropeBloodAmount <= 0f;
        public float FillAmount() => lycanthropeBloodAmount;
        public bool CanAddBlood(float addAmount = defaultChangeAmount) => lycanthropeBloodAmount + addAmount <= maxBloodAmount;
        public bool CanConsumeBlood(float consumeAmount = defaultChangeAmount) => lycanthropeBloodAmount - consumeAmount >= 0f;

        public float AddBlood(float addAmount = defaultChangeAmount)
        {
            lycanthropeBloodAmount += addAmount;
            return addAmount;
        }

        public void ConsumeBlood(float consumeAmount = defaultChangeAmount)
        {
            lycanthropeBloodAmount -= consumeAmount;
        }

        public override string GetInspectString()
        {
            return "Mashed_Bloodmoon_AltarFillAmount".Translate(lycanthropeBloodAmount.ToStringPercent()).CapitalizeFirst();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref lycanthropeBloodAmount, "lycanthropeBloodAmount", 0f);
            base.ExposeData();
        }
    }
}
