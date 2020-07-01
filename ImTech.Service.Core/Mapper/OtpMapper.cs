using ImTech.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using am = AutoMapper;
using ImTech.Model;
namespace ImTech.Service.Mapper
{
    public class OtpMapper
    {
        private ViewModelFactory _viewModelFactory;
        public OtpMapper(ViewModelFactory viewModelFactory)
        {
            this._viewModelFactory = viewModelFactory;

        }
        public otpViewmodel Map(OTPModel oTPModel)
        {
            return new otpViewmodel
            {
                otpCode = oTPModel.otpCode,
                UserId = oTPModel.UserId,
                otpMobileNo = oTPModel.otpMobileNo
            };
        }

        public OTPModel Map(otpViewmodel oTPViewModel)
        {
            return new OTPModel
            {
                otpCode = oTPViewModel.otpCode,
                UserId = oTPViewModel.UserId,
                otpMobileNo = oTPViewModel.otpMobileNo
            };
        }
    }
}