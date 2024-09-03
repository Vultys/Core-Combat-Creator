using System.Collections.Generic;
using UnityEngine;

namespace CCC.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifier(Stat stat); 
    }
}
