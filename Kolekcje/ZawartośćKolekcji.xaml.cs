using System.Collections.ObjectModel;

namespace Kolekcje;

public partial class ZawartoœæKolekcji: ContentPage
{
    public ObservableCollection<ElementKolekcji> ZawartoscKolekcji {  get; set; }
    public Kolekcje.MainPage MainPage { get; set; }
    public int kolekcjaId { get; set; }
    public ZawartoœæKolekcji(int id, Kolekcje.MainPage mainPage)
	{
		InitializeComponent();
        this.kolekcjaId = id;   
        this.MainPage = mainPage;
		Label.Text = id.ToString();
        ZawartoscKolekcji = mainPage.Kolekcje.Where(item => item.Id == id).FirstOrDefault().elementy;
        BindingContext = this;
        collectionView.ItemTemplate = new DataTemplate(() => {
            var button = new Button();
            button.SetBinding(Button.TextProperty, "Nazwa");
            return button;
        });

   
    }

    private async void NowyElement_Clicked(object sender, EventArgs e)
    {
        string nazwa = await DisplayPromptAsync("WprowadŸ nazwê", "Nazwa: ");
        if (nazwa != null)
        {
            int id = 0;
            if (ZawartoscKolekcji.Count > 0)
            {
               id = ZawartoscKolekcji[ZawartoscKolekcji.Count - 1].Id + 1;
            }
            else
            {
                id = kolekcjaId * 100;
            }
            var elementKolekcji = new ElementKolekcji(id, nazwa);
            ZawartoscKolekcji.Add(elementKolekcji);
            //MainPage.Kolekcje.Where(item => item.Id == kolekcjaId).FirstOrDefault().elementy.Add(elementKolekcji);
            MainPage.ZapiszDane();
            
        }
    }


}