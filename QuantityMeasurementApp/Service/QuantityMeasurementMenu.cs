using QuantityMeasurementApp.Controller;
using QuantityMeasurementApp.Interfaces;

namespace QuantityMeasurementApp.Menu
{
    /// <summary>
    /// Console menu implementation.
    /// </summary>
    public class QuantityMeasurementMenu : IMenu
    {
        private readonly QuantityMeasurementController controller;

        public QuantityMeasurementMenu()
        {
            controller = new QuantityMeasurementController();
        }

        public void Show()
        {
            controller.ShowMainMenu();
        }
    }
}