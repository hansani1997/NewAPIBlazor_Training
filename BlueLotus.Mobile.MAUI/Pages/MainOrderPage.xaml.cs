using BlueLotus.Mobile.MAUI.BLControls.Product;
using BlueLotus.Mobile.MAUI.Pages.BasePage;
using BlueLotus.Mobile.MAUI.Pages.PopUps;
using BlueLotus.Mobile.MAUI.UIBuilder;
using BlueLotus.Mobile.MAUI.ViewModels;
using BlueLotus.Mobile.MAUI.ViewModels.Category;
using BlueLotus.Mobile.MAUI.ViewModels.HomePage;
using BlueLotus.UI.Application.Services.Defintions;
using BlueLotus360.Core.Domain.DTOs.RequestDTO;
using BlueLotus360.Core.Domain.Entity.Base;
using BlueLotus360.Core.Domain.Entity.Object;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

namespace BlueLotus.Mobile.MAUI.Pages;

[QueryProperty("Menu", "Menu")]
public partial class MainOrderPage : ContentPage
{
    protected BaseViewModel __bindContext;
    protected readonly IAppObjectService _objectAppService;
    protected readonly ICodeBaseService _codeBaseService;

    private BLUIElement _categoryPage;
    private BLUIElement _orderPage;
    private BLUIElement _customerPage;
    private CategoryViewModel SelectedCategory;
    public UIMenu Menu { get; set; }

    public MainOrderPage()
    {
        _objectAppService = MauiProgram.Services.GetService<IAppObjectService>();
        _codeBaseService = MauiProgram.Services.GetService<ICodeBaseService>();
        __bindContext = new();

        InitializeComponent();
    }
    private async Task ReadCategories()
    {
        if (_categoryPage != null)
        {
            ComboRequestDTO dto = new ComboRequestDTO();
            dto.RequestingElementKey = (int)_categoryPage.ElementKey;
            var items = await _codeBaseService.ReadProductCategories(dto);
            if (items.Value != null)
            {
                var width = 400;
                __categoryPage.Clear();
                foreach (var item in items.Value)
                {
                    CategoryViewModel model = new CategoryViewModel();
                    model.CodeKey = item.CodeKey;
                    model.CategoryName = item.CodeName;
                    model.ImagePathName = string.IsNullOrWhiteSpace(item.CodeExtraCharacter1) ? "no_image.png" : item.CodeExtraCharacter1;
                    var catm = new CategoryView(model);
                    catm.WidthRequest = width * 0.97;
                    catm.CategoryClickEvent += Catm_CategoryClickEvent;
                    __categoryPage.Add(
                      catm
                        );
                }
            }
        }
    }

    private async void Catm_CategoryClickEvent(object sender, Events.CategoryClickEventArgs e)
    {
        SelectedCategory = e.Category;
       SelectedCategoryName.Text = "Products Under Category - " + SelectedCategory.CategoryName + ".";
        __categoryPage.RotateXTo(30);
        await __categoryPage.FadeTo(0);

        __productPage.IsVisible = true;
        __categoryPage.IsVisible = false;
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
       
        base.OnNavigatingFrom(args);
    }

    private async Task LoadProducts()
    {
        await Task.CompletedTask;
    }


    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
      

        if (Menu==null &&  BindingContext != null && BindingContext.GetType() == typeof(UIMenu))
        {
            Menu = (UIMenu)BindingContext;
            BindingContext = null;
            BindingContext = __bindContext;
        }
        
       
        if (Menu != null)
        {
            var shellModel = MauiProgram.Services.GetService<AppShellModel>();
            if (shellModel != null)
            {
                shellModel.ShellTitle = Menu.MenuCaption;
            }

            var elem = await _objectAppService.FetchObjects(Menu);
            foreach (var obj in elem.Value.Children)
            {
                if (obj.ElementName.Equals("__ProductsPage__"))
                {
                    _categoryPage = obj;
                }
            }
        }

        base.OnNavigatedTo(args);

        await ReadCategories();
    }


    protected async void OnBackButtonClicked(object sender, EventArgs args)
    {
        SelectedCategory = null;
        __productPage.IsVisible = false;
        __categoryPage.IsVisible = true;
        __categoryPage.FadeTo(1);
        await __categoryPage.RotateXTo(0);

    }

    protected async void OnCustomerSelectClick(object sender, EventArgs args)
    {
      AddressSelectPopUp pop = new AddressSelectPopUp();
        this.ShowPopup(pop);

    }


}