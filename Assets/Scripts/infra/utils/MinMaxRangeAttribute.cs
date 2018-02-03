using System;

namespace Infra.Utils {
/// <summary>
/// This attribute needs to be enforced by property drawers.
/// It is now used by the RangedFloatDrawer.
/// </summary>
public class MinMaxRangeAttribute : Attribute {
    public MinMaxRangeAttribute(float min, float max) {
        Min = min;
        Max = max;
    }

    public float Min { get; private set; }
    public float Max { get; private set; }
}
}