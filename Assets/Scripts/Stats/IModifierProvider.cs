using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifier(Stats stat);
        IEnumerable<float> GetPercentageModifier(Stats stat);
    }
}