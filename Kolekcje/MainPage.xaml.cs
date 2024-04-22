using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;


namespace Kolekcje
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Kolekcja> Kolekcje { get; set; }  

        public MainPage()
        {
            System.Diagnostics.Debug.WriteLine("Rozpoczynam działanie");
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("Rozpoczynam ładowanie danych");
            WczytajDane();
            System.Diagnostics.Debug.WriteLine("Zakończono ładowanie danych");
            BindingContext = this;
            mineCollectionView.ItemTemplate = new DataTemplate(() => {
                var button = new Button();
                button.SetBinding(Button.TextProperty, "Nazwa");
                button.Clicked += async (sender, e) => {
                    var kolekcja = (sender as Button).BindingContext as Kolekcja; 
                    await Navigation.PushAsync(new ZawartośćKolekcji(kolekcja.Id, this));
                };
                return button;
            });
        }

        private async void NowaKolekcja_Clicked(object sender, EventArgs e)
        {
            string nazwa = await DisplayPromptAsync("Wprowadź nazwę", "Nazwa: ");
            if (nazwa != null) {
                int id = Kolekcje[Kolekcje.Count - 1].Id + 1;
                Kolekcje.Add(new Kolekcja(id, nazwa));
                ZapiszDane();
            }
        }

        public void ZapiszDane()
        {
            string plik = "C:\\dev\\Kolekcje\\Kolekcje\\dane.csv";
            if (File.Exists(plik))
            {

                using (StreamWriter writer = new StreamWriter(plik))
                {
                    foreach (var item in Kolekcje)
                    {
                        writer.WriteLine($"{item.Id};0;{item.Nazwa}");
                 
                    }
                    foreach(var item in Kolekcje)
                    {
                        foreach(var element in item.elementy)
                        {
                            writer.WriteLine($"{element.Id};{item.Id};{element.Nazwa}");
                        }
                    }
                }

            }
        }
        private void WczytajDane ()
        {
            Kolekcje = new ObservableCollection<Kolekcja>();
            try {
                string plik = "C:\\dev\\Kolekcje\\Kolekcje\\dane.csv";
                if (File.Exists(plik)) {
                    System.Diagnostics.Debug.WriteLine("Znaleziono plik");
                    string[] linie = File.ReadAllLines(plik);
                    foreach (string linia in linie) {
                        string[] elementy = linia.Split(';');
                        int id = int.Parse(elementy[0]);
                        int idRodzica = int.Parse(elementy[1]);
                        if (idRodzica == 0)
                        {


                            Kolekcja kolekcja = new Kolekcja(id, elementy[2]);
                            Kolekcje.Add(kolekcja);
                        }
                        else
                        {
                            Kolekcje[idRodzica - 1].elementy.Add(new ElementKolekcji(id, elementy[2]));
                        }
                        System.Diagnostics.Debug.WriteLine("Dodano kolekcje " + id + ", " + elementy[1]);
                    }
                } else {
                    System.Diagnostics.Debug.WriteLine("Nie znaleziono pliku");
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

    }

    public class Kolekcja { 
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public ObservableCollection<ElementKolekcji> elementy { get; set; }
        public Kolekcja (int id, string nazwa)
        {
            Id = id;
            Nazwa = nazwa;
            elementy = new ObservableCollection<ElementKolekcji>();
        }
    }

    public class ElementKolekcji
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public ElementKolekcji(int id, string nazwa)
        {
            Id = id;
            Nazwa = nazwa;
        }
    }
}