using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Mashed_Bloodmoon
{
    public class LTRWorker_Meme : LycanthropeTypeRequirementWorker
    {
        public override AcceptanceReport PawnRequirementsMet(Pawn pawn)
        {
            if (ModsConfig.IdeologyActive && pawn.Ideo != null)
            {
                if (memeDef != null && pawn.Ideo.HasMeme(memeDef))
                {
                    return true;
                }
                if (!memeDefs.NullOrEmpty())
                {
                    foreach (MemeDef def in memeDefs)
                    {
                        if (pawn.Ideo.HasMeme(def))
                        {
                            return true;
                        }
                    }
                }
            }
            return "Mashed_Bloodmoon_LTR_InvalidMeme".Translate();
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (memeDef == null && memeDefs.NullOrEmpty())
            {
                yield return "both memeDef and memeDefs are null";
            }
            if (memeDef != null && !memeDefs.NullOrEmpty())
            {
                yield return "use either memeDef or memeDefs";
            }
        }

        public MemeDef memeDef;
        public List<MemeDef> memeDefs;
    }
}
