using System.Xml;
using Verse;

namespace Mashed_Bloodmoon
{
    public class PawnTotemRecord
    {
        public TotemTypeDef totemTypeDef;
        public IntRange startingCountRange;

        public void LoadDataFromXmlCustom(XmlNode xmlRoot)
        {
            DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "totemTypeDef", xmlRoot);
            startingCountRange = ParseHelper.FromString<IntRange>(xmlRoot.FirstChild.Value);
        }
    }
}
