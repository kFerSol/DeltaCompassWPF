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

            switch (gameName)
            {
                case "Apex Legends":
                    imagePath = "pack://application:,,,/resource/apex-logo.png";
                    break;
                case "Counter Strike 2":
                    imagePath = "pack://application:,,,/resource/cslogo.png";
                    break;
                case "Call of Duty MWIII":
                    imagePath = "pack://application:,,,/resource/MWIII-logo.png";
                    break;
                case "Portal 2":
                    imagePath = "pack://application:,,,/resource/portal-logo.png";
                    break;
                case "Rainbow Six Siege":
                    imagePath = "pack://application:,,,/resource/rainbow-logo.jpg";
                    break;
                case "Valorant":
                    imagePath = "pack://application:,,,/resource/valorant-logo.png";
                    break;
                default:
                    imagePath = "pack://application:,,,/resource/delta-logo.png";
                    break;
            }

            return new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }
}
