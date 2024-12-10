using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using No_Mole.Converters;

namespace No_Mole.Converters
{
    internal class SliderValueToHeightConverter : IValueConverter
    {
        /// <summary>
        /// Converts the slider's value to a proportional height.
        /// </summary>
        /// <param name="value">The slider's current value.</param>
        /// <param name="targetType">The target property type (should be double).</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">The culture info (not used).</param>
        /// <returns>The calculated height.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double sliderValue = System.Convert.ToDouble(value);
            double maxHeight = 400; // Height of the slider
            double minValue = 1.0;
            double maxValue = 4.0;

            // Scale the height proportionally
            return (sliderValue - minValue) / (maxValue - minValue) * maxHeight; // Default height in case of invalid input
        }

        /// <summary>
        /// ConvertBack is not implemented since it's not needed for one-way binding.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
