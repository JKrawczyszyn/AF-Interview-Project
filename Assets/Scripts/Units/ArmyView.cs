namespace AFSInterview.Units
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class ArmyView : MonoBehaviour
    {
        private List<UnitPresenter> units;

        private void Awake()
        {
            units = GetComponentsInChildren<UnitPresenter>().ToList();

            units.ForEach(Initialize);
            units.ForEach(ResetState);
        }

        private void Initialize(UnitPresenter presenter)
        {
            presenter.Initialize(this);
        }

        private void ResetState(UnitPresenter presenter)
        {
            presenter.GetUnit().ResetState();
        }

        public void RemoveUnit(UnitPresenter presenter)
        {
            units.Remove(presenter);
        }

        public IEnumerable<Unit> GetUnits()
        {
            return units.Select(u => u.GetUnit());
        }

        public string GetUnitName(Unit unit)
        {
            return GetUnitPresenter(unit)?.name;
        }

        public UnitPresenter GetUnitPresenter(Unit unit)
        {
            return units.FirstOrDefault(u => u.GetUnit() == unit);
        }
    }
}
