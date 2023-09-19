using IA_I.UI;
using UnityEngine;

namespace IA_I.Damageables
{
    public abstract class Damageable : MonoBehaviour
    {
        protected SliderComponent _slider;

        protected virtual void Awake()
        {
            _slider = GetComponentInChildren<SliderComponent>();
        }
    }
}
