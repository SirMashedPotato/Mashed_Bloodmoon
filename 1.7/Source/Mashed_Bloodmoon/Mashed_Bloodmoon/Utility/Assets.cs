using UnityEngine;
using Verse;

namespace Mashed_Bloodmoon
{
    [StaticConstructorOnStartup]
    public static class Assets
    {
        public static readonly Texture2D ConsumedHeartsFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.7f, 0.3f, 0.3f));
        public static float RectPadding = 12f;
    }
}
