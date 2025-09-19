using RimWorld;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace Mashed_Bloodmoon
{
    public class Alert_IdleTransformedLycanthrope : Alert
    {
        private readonly List<Pawn> idleLycanthropeColonists = new List<Pawn>();
        private readonly StringBuilder sb = new StringBuilder();

        private List<Pawn> IdleLycanthropeColonists
        {
            get
            {
                idleLycanthropeColonists.Clear();
                foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsAndPrisonersSpawned)
                {
                    if (LycanthropeUtility.PawnIsTransformedLycanthrope(pawn) && pawn.mindState.IsIdle)
                    {
                        idleLycanthropeColonists.Add(pawn);
                    }
                }
                return idleLycanthropeColonists;
            }
        }

        public override string GetLabel()
        {
            if (idleLycanthropeColonists.Count == 1)
            {
                return "Mashed_Bloodmoon_IdleTransformedLycanthrope_Label".Translate();
            }
            return "Mashed_Bloodmoon_IdleTransformedLycanthropes_Label".Translate(idleLycanthropeColonists.Count.ToStringCached());
        }

        public override TaggedString GetExplanation()
        {
            sb.Length = 0;
            foreach (Pawn item in idleLycanthropeColonists)
            {
                sb.AppendLine("  - " + item.NameShortColored.Resolve());
            }
            return "Mashed_Bloodmoon_IdleTransformedLycanthrope_Desc".Translate(sb.ToString().TrimEndNewlines());
        }

        public override AlertReport GetReport()
        {
            return AlertReport.CulpritsAre(IdleLycanthropeColonists);
        }
    }
}
