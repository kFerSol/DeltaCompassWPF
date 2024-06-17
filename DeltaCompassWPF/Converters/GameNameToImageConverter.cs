using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DeltaCompassWPF.Converters
{
    public class GameNameToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            string gameName = value as string;

            if (gameName == null)
                return null;

            string imagePath;

            if (gameName == "Apex Legends")
            {
                imagePath = "../resource/apex-logo.png";
            }
            else if (gameName == "Counter Strike 2")
            {
                imagePath = "../resource/cslogo.png";
            }
            else if (gameName == "Call of Duty MWIII")
            {
                imagePath = "../resource/MWIII.png";
            }
            else if (gameName == "Portal 2")
            {
                imagePath = "../resource/portal-logo.png";
            }
            else if (gameName == "Rainbow Six Siege")
            {
                imagePath = "../resource/rainbow-logo.jpg";
            }
            else if (gameName == "Valorant")
            {
                imagePath = "../resource/valorant-logo.png";
            }
            else
            {
                imagePath = "../resource/delta-logo.png"; 
            }

            return new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }
}
