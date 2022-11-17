using BlueLotus.Mobile.MAUI.BLControls.Product;
using BlueLotus.Mobile.MAUI.ViewModels.Category;
using BlueLotus.UI.Application.Context;

namespace BlueLotus.Mobile.MAUI.Pages;


[QueryProperty("SelectedCategory", "SelectedCategory")]
public partial class ProductListPage : ContentPage
{

	public CategoryViewModel SelectedCategory { get; set; }

	public ProductListPage()
	{
		InitializeComponent();
	}
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (SelectedCategory != null)
        {
            await LoadProducts();
        }
    }

    private async Task LoadProducts()
    {
        var appContext = MauiProgram.Services.GetService<BLUIAppContext>();
        var listProducts = appContext.FilterItemByCat(SelectedCategory.CodeKey);
        SelectedCategoryName.Text = $"Products Under Category - {SelectedCategory.CategoryName} ({listProducts.Count})";
        __productListView.Clear();
        foreach (var item in listProducts)
        {
            ProductViewModel model = new ProductViewModel();
            model.ProductName = item.ItemName;
            model.SalesPrice = item.SalesPrice;
            model.ItemKey = item.ItemKey;
            model.ImagePathName = item.Base64ImageDocument;
            model.Category = SelectedCategory;
            model.Description = item.Description;

            ProductView view = new ProductView(model);
            view.ProductClickEvent += View_ProductClickEvent; ;
            __productListView.Add(view);
        }
        await Task.CompletedTask;
    }

    private void View_ProductClickEvent(object sender, Events.ProductClickEventArgs e)
    {
      
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate(); 
     
    }
}