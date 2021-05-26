using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Alauda
{
    /// <summary>
    /// 图标按钮
    /// </summary>
    public class IconButton : Button
    {
        public static DependencyProperty NormalImageProperty = DependencyProperty.Register("NormalImage", typeof(ImageSource), typeof(IconButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, null));
        public static DependencyProperty PressedImageProperty = DependencyProperty.Register("PressedImage", typeof(ImageSource), typeof(IconButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, null));
        public static DependencyProperty HoverImageProperty = DependencyProperty.Register("HoverImage", typeof(ImageSource), typeof(IconButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, null));
        public static DependencyProperty DisabledImageProperty = DependencyProperty.Register("DisabledImage", typeof(ImageSource), typeof(IconButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, null));
        public static DependencyProperty ImageSizeProperty = DependencyProperty.Register("ImageSize", typeof(double), typeof(IconButton));


        static IconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
        }


        public ImageSource NormalImage
        {
            get => (ImageSource)GetValue(NormalImageProperty);
            set => SetValue(NormalImageProperty, value);
        }


        public ImageSource PressedImage
        {
            get => (ImageSource)GetValue(PressedImageProperty);
            set => SetValue(PressedImageProperty, value);
        }

        public ImageSource HoverImage
        {
            get => (ImageSource)GetValue(HoverImageProperty);
            set => SetValue(HoverImageProperty, value);
        }

        public ImageSource DisabledImage
        {
            get => (ImageSource)GetValue(DisabledImageProperty);
            set => SetValue(DisabledImageProperty, value);
        }


        public double ImageSize
        {
            get => (double)GetValue(ImageSizeProperty);
            set => SetValue(ImageSizeProperty, value);
        }
    }
}
