using Xamarin.Forms;

namespace MyCustomPicker.Controls
{
    public class CustomPicker : Picker
    {
        public static readonly BindableProperty ItemFontFamilyProperty =
                BindableProperty.Create("ItemFontFamily", typeof(string), typeof(CustomPicker), defaultBindingMode: BindingMode.OneWay);

        public string ItemFontFamily
        {
            get { return (string)GetValue(ItemFontFamilyProperty); }
            set { SetValue(ItemFontFamilyProperty, value); }
        }

        public static readonly BindableProperty ItemColorProperty =
                BindableProperty.Create("ItemColor", typeof(string), typeof(CustomPicker), defaultBindingMode: BindingMode.OneWay);

        public string ItemColor
        {
            get { return (string)GetValue(ItemColorProperty); }
            set { SetValue(ItemColorProperty, value); }
        }
    }
}
