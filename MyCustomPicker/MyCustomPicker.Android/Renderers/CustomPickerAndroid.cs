using System;
using System.Linq;
using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;
using Android.Graphics;

using MyCustomPicker.Controls;
using MyCustomPicker.Droid.Renderers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using static Android.Widget.AdapterView;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerAndroid))]
namespace MyCustomPicker.Droid.Renderers
{
    public class CustomPickerAndroid : PickerRenderer
    {
        private Dialog dialog;

        public CustomPickerAndroid() { }

        private string itemFont;
        private string itemColor;
        private string title;
        private Android.Graphics.Color titleColor;
        private BindingBase displayText;

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            Control.Click += Control_Click;

            var customPicker = 
                (e.NewElement != null) ? (CustomPicker)e.NewElement
                : (e.OldElement != null) ? (CustomPicker)e.OldElement
                : new CustomPicker();

            itemFont = !string.IsNullOrWhiteSpace(customPicker.ItemFontFamily) 
                ? customPicker.ItemFontFamily : "Pacifico";

            itemColor = !string.IsNullOrWhiteSpace(customPicker.ItemColor) 
                ? customPicker.ItemColor : "#0000ff";

            title = !string.IsNullOrWhiteSpace(customPicker.Title) 
                ? customPicker.Title : "Select an item";

            titleColor = customPicker.TitleColor.ToAndroid();
            displayText = customPicker.ItemDisplayBinding;
        }

        protected override void Dispose(bool disposing)
        {
            Control.Click -= Control_Click;
            base.Dispose(disposing);
        }

        private void Control_Click(object sender, EventArgs e)
        {
            var model = Element;
            dialog = new Dialog(Forms.Context);
            dialog.SetContentView(Resource.Layout.custom_picker_dialog);

            var textView = (TextView)dialog.FindViewById(Resource.Id.titletextview);
            textView.Text = title;
            textView.SetTextColor(titleColor);

            var items = new List<object>();
            foreach (var item in model.ItemsSource)
                items.Add(item);

            var listView = (Android.Widget.ListView)dialog.FindViewById(Resource.Id.listview);
            listView.Adapter = new MyAdapter(items, itemFont, itemColor, displayText);

            listView.ItemClick += (object sender1, ItemClickEventArgs e1) =>
            {
                Element.SelectedIndex = e1.Position;
                dialog.Hide();
            };

            if (model.ItemsSource.Count > 3)
            {
                var height = Xamarin.Forms.Application.Current.MainPage.Height;
                var width = Xamarin.Forms.Application.Current.MainPage.Width;
                dialog.Window.SetLayout(700, 800);
            }

            dialog.Show();
        }

        class MyAdapter : BaseAdapter
        {
            private IList<object> mList;
            private Typeface mFont;
            private Android.Graphics.Color mColor;
            private string mDisplay;

            public MyAdapter(IList<object> itemsSource, string font, string color, BindingBase display)
            {
                mList = itemsSource;
                mFont = Typeface.CreateFromAsset(Forms.Context.Assets, font.Split('#')[0]);
                mColor = Android.Graphics.Color.ParseColor(color);
                mDisplay = ((Binding)display).Path;
            }

            public override int Count => mList.Count;

            public override Java.Lang.Object GetItem(int position)
            {
                var myObj = mList.ElementAt(position);
                return new JavaObjectWrapper<object>() { Obj = myObj };
            }

            public override long GetItemId(int position)
            {
                return position;
            }

            public override Android.Views.View GetView(int position, Android.Views.View view, ViewGroup parent)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.cell_layout, null);

                var text = view.FindViewById<TextView>(Resource.Id.textview1);
                text.Typeface = mFont;
                text.SetTextColor(mColor);

                var obj = mList.ElementAt(position);
                text.Text = obj.GetType().GetProperty(mDisplay).GetValue(obj, null).ToString();

                return view;
            }
        }

        public class JavaObjectWrapper<T> : Java.Lang.Object
        {
            public T Obj { get; set; }
        }
    }
}