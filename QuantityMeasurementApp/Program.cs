using QuantityMeasurementApp.Menu;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            QuantityMeasurementMenu menu = new QuantityMeasurementMenu();
            menu.Show();
        }
    }
}