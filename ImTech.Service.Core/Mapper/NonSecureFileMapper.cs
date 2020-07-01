using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ImTech.Model;
using am = AutoMapper;
using ImTech.Service.ViewModels;

namespace ImTech.Service.Mapper
{
    public class NonSecureFileMapper
    {
        private ViewModelFactory _viewModelFactory;
        public NonSecureFileMapper(ViewModelFactory viewModelFactory)
        {
            this._viewModelFactory = viewModelFactory;
        }

        #region Mapping Methods

        public NonSecureFileViewModel Map(NonSecureFileModel secureModel)
        {
            var vm = am.Mapper.Map<NonSecureFileViewModel>(secureModel);
            return vm;
        }

        public IEnumerable<NonSecureFileViewModel> Map(IEnumerable<NonSecureFileModel> secureViewModels)
        {
            return secureViewModels.Select(Map);

        }
        public NonSecureFileModel Map(NonSecureFileViewModel secureViewModel)
        {
            return am.Mapper.Map<NonSecureFileModel>(secureViewModel);
        }

        public IEnumerable<NonSecureFileModel> Map(IEnumerable<NonSecureFileViewModel> secureModels)
        {
            return secureModels.Select(Map);
        }

        #endregion

        public NonSecureFileViewModel CreateViewModel()
        {
            return this._viewModelFactory.CreateNonSecureFileViewModel();
        }
    }
}