using QuantityMeasurementApp.ModelLayer.Enums;

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    public static class VolumeUnit
    {
        public static double ConvertToBaseUnit(VolumeEnum unit, double value)
        {
            return unit switch
            {
                VolumeEnum.LITRE      => value,
                VolumeEnum.MILLILITRE => value * 0.001,
                VolumeEnum.GALLON     => value * 3.78541,
                _                     => throw new System.Exception("Invalid volume unit")
            };
        }

        public static double ConvertFromBaseUnit(VolumeEnum unit, double baseValue)
        {
            return unit switch
            {
                VolumeEnum.LITRE      => baseValue,
                VolumeEnum.MILLILITRE => baseValue * 1000,
                VolumeEnum.GALLON     => baseValue / 3.78541,
                _                     => throw new System.Exception("Invalid volume unit")
            };
        }
    }
}