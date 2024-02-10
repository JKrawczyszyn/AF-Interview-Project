namespace AFSInterview.Units
{
    using UnityEngine;

    /// <summary>
    /// Presenter for the Unit.
    /// </summary>
    public class UnitPresenter : MonoBehaviour
    {
        [SerializeField] private Unit unit;

        private ArmyView view;

        public void Initialize(ArmyView view)
        {
            this.view = view;
        }

        public void DestroySelf()
        {
            view.RemoveUnit(this);

            Destroy(gameObject);
        }

        public Unit GetUnit()
        {
            return unit;
        }
    }
}
