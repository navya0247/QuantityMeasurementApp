using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Models;
using QuantityMeasurementApp.RepoLayer.Repositories;

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    /// <summary>
    /// Facade service used by controllers or UI layer.
    /// Handles length comparison, conversion, and arithmetic operations.
    /// </summary>
    public static class QuantityMeasurementService
    {
        private static QuantityMeasurementServiceImpl service =
            new QuantityMeasurementServiceImpl(QuantityMeasurementCacheRepository.GetInstance());

        public static bool AreFeetEqual(double a, double b)
        {
            var q1 = new QuantityDTO(a, LengthEnum.FEET);
            var q2 = new QuantityDTO(b, LengthEnum.FEET);

            return service.Compare(q1, q2);
        }

        public static bool AreInchesEqual(double a, double b)
        {
            var q1 = new QuantityDTO(a, LengthEnum.INCHES);
            var q2 = new QuantityDTO(b, LengthEnum.INCHES);

            return service.Compare(q1, q2);
        }

        public static bool AreLengthEqual(double v1, LengthEnum u1, double v2, LengthEnum u2)
        {
            var q1 = new QuantityDTO(v1, u1);
            var q2 = new QuantityDTO(v2, u2);

            return service.Compare(q1, q2);
        }

        public static double ConvertLength(double value, LengthEnum from, LengthEnum to)
        {
            var q = new QuantityDTO(value, from);

            var result = service.Convert(q, to);

            return result.Value;
        }

        public static QuantityDTO AddLengths(double v1, LengthEnum u1, double v2, LengthEnum u2)
        {
            var q1 = new QuantityDTO(v1, u1);
            var q2 = new QuantityDTO(v2, u2);

            return service.Add(q1, q2, u1);
        }

        public static QuantityDTO AddLengths(double v1, LengthEnum u1, double v2, LengthEnum u2, LengthEnum target)
        {
            var q1 = new QuantityDTO(v1, u1);
            var q2 = new QuantityDTO(v2, u2);

            return service.Add(q1, q2, target);
        }
    }
}