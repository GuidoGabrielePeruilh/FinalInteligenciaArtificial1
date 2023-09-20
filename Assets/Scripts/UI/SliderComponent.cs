using UnityEngine;
using UnityEngine.UI;

namespace IA_I.UI
{
    public class SliderComponent : MonoBehaviour
    {
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }
        public void UpdateSlider(float currentLife, float maxLife)
        {
            _slider.value = currentLife / maxLife;
        }
    }
}
