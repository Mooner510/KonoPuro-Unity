using UnityEngine.UI;

namespace _root.Script.Manager
{
    public static class UIColor
    {
        public static void Alpha(this Graphic graphic, float alpha)
        {
            var color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }
}