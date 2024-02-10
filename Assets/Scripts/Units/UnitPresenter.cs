namespace AFSInterview.Units
{
    using UnityEngine;

    public class UnitPresenter : MonoBehaviour
    {
        [SerializeField] private Unit unit;

        private void Awake()
        {
            unit.OnDied += DestroySelf;
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }

        public Unit GetUnit()
        {
            return unit;
        }
    }
}
