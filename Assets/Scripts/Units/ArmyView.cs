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

            units.ForEach(ResetState);
            units.ForEach(AddOnDiedHandler);
        }

        private void ResetState(UnitPresenter unit)
        {
            unit.GetUnit().ResetState();
        }

        private void AddOnDiedHandler(UnitPresenter unit)
        {
            unit.GetUnit().OnDied += () => units.Remove(unit);
        }

        public IEnumerable<Unit> GetUnits()
        {
            return units.Select(u => u.GetUnit());
        }

        public string GetUnitName(Unit unit)
        {
            return units.Where(u => u.GetUnit() == unit).Select(u => u.name).FirstOrDefault();
        }
    }
}
