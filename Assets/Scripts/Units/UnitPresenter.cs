namespace AFSInterview.Units
{
    using UnityEngine;

    public class UnitPresenter : MonoBehaviour
    {
        [SerializeField] private Unit unit;

        public Unit GetUnit(bool disposeHolder)
        {
            if (disposeHolder)
                Destroy(gameObject);

            return unit;
        }
    }
}
