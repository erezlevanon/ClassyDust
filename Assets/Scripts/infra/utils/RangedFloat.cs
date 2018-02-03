using System;

namespace Infra.Utils {
/// <summary>
/// This struct is displayed as a min-max slider.
/// </summary>
[Serializable]
public struct RangedFloat {
    public float minValue;
    public float maxValue;

    public RangedFloat(float defaultValue) {
        minValue = defaultValue;
        maxValue = defaultValue;
    }

    public RangedFloat(float minValue, float maxValue) {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public float Random() {
        return UnityEngine.Random.Range(minValue, maxValue);
    }
}
}