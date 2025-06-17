using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Mashed_Bloodmoon
{
    public class JobDriver_CustomiseBeastForm : JobDriver_WolfsbloodAltar
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
            yield return Toils_General.Do(delegate
            {
                Page_CustomiseBeastForm page = new Page_CustomiseBeastForm(LycanthropeUtility.GetCompLycanthrope(pawn));
                Find.WindowStack.Add(page);
            });
        }
    }
}
