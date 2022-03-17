using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PhoneWord
{
    internal class MainPage : ContentPage
    {

        Entry phoneNumberText;
        Button translateButton;
        Button callButton;
        string translatedNumber;
        public MainPage()
        {
            this.Padding = new Thickness(20, 20, 20, 20);

            StackLayout panel = new StackLayout
            {
                Spacing = 15
            };

            panel.Children.Add(new Label
            {
                Text = "Añade un Numero y texto:",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });

            panel.Children.Add(phoneNumberText = new Entry
            {
                Text = "1-855-XAMARIN",
            });

            panel.Children.Add(translateButton = new Button
            {
                Text = "Traducir"
            });

            panel.Children.Add(callButton = new Button
            {
                Text = "Llamar",
                IsEnabled = false,
            });

            this.Content = panel;

            translateButton.Clicked += OnTranslate;
            this.Content = panel;

            translateButton.Clicked += OnTranslate;
            callButton.Clicked += OnCall;
            this.Content = panel;
        }

        private void OnTranslate(object sender, EventArgs e)
        {
            string enteredNumber = phoneNumberText.Text;
            translatedNumber = PhonewordTraductor.ToNumber(enteredNumber);

            if (!string.IsNullOrEmpty(translatedNumber))
            {
                callButton.IsEnabled = true;
                callButton.Text = "Llamar " + translatedNumber;
            }
            else
            {
                callButton.IsEnabled = false;
                callButton.Text = "Llamar";
            }
        }

        async void OnCall(object sender, System.EventArgs e)
        {
            if (await this.DisplayAlert(
            "Marcar un Numero",
            "¿Te gustaría llamar? " + translatedNumber + "?",
            "Si",
            "No"))
            {
                try
                {
                    PhoneDialer.Open(translatedNumber);
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("No se pudo llamar", "El numero no era valido.", "OK");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("No se pudo llamar", "Telefono no admitido.", "OK");
                }
                catch (Exception)
                {
                    // Other error has occurred.
                    await DisplayAlert("No se pudo llamar", "La llamada fallo", "OK");
                }
            }
        }
    }
}
