using BlueLotus.Mobile.MAUI.Context;
using BlueLotus.Mobile.MAUI.Pages;
using BlueLotus.Mobile.MAUI.Validators.UserAuthentication;
using BlueLotus.UI.Application.Context;
using BlueLotus.UI.Application.Services.Defintions;
using BlueLotus360.Core.Domain.Models;
using BlueLotus360.Core.Domain.Responses;
using BlueLotus360.Data.APIConsumer.APIConsumer.RestAPIConsumer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlueLotus.Mobile.MAUI.ViewModels.UserAuthentication
{
    public partial class UserLoginModel : BaseViewModel
    {
        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string password;


        [RelayCommand]
        private async Task OnLoginClick()
        {
            UserAuthenticationValidator validator = new UserAuthenticationValidator();
            ValidationResult result = validator.Validate(this);
            if (result.IsValid)
            {
                UserAuthenticationRequest request = new UserAuthenticationRequest();
                request.UserName = userName;
                request.Password = password;
                BaseServerResponse<UserAuthenticationResponse> response = await _userService.AuthenticateUserAsync(request);
                if (response.Value != null)
                {
                    if (response.Value.IsSuccess)
                    {                       
                        Application.Current.MainPage = MauiProgram.Services.GetService<CompanySelectionPage>();


                    }
                }
            }
            else
            {

            }

        }

        private readonly IAppUserService _userService;
        private readonly BLUIAppContext _appContext;

        public UserLoginModel(IAppUserService service,BLUIAppContext appContext)
        {
            _userService = service;
            _appContext = appContext;
        }

    }
}
