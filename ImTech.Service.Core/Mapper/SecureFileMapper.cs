using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ImTech.Model;
using am = AutoMapper;
using ImTech.Service.ViewModels;

namespace ImTech.Service.Mapper
{
    public class SecureFileMapper
    {
        private ViewModelFactory _viewModelFactory;
        public SecureFileMapper(ViewModelFactory viewModelFactory)
        {
            this._viewModelFactory = viewModelFactory;
        }

        #region Mapping Methods

        public SecureFileViewModel Map(SecureFileModel secureModel)
        {
            return new SecureFileViewModel
            {
                ActualFileName = secureModel.ActualFileName,
                CreatedBy = secureModel.CreatedBy,
                Id = secureModel.Id
            };
        }

        public IEnumerable<SecureFileViewModel> Map(IEnumerable<SecureFileModel> secureViewModels)
        {
            return secureViewModels.Select(Map);

        }
        public SecureFileModel Map(SecureFileViewModel secureViewModel)
        {
            return new SecureFileModel
            {
                ActualFileName = secureViewModel.ActualFileName,
                CreatedBy = secureViewModel.CreatedBy,
                Id = secureViewModel.Id
            };
        }

        public IEnumerable<SecureFileModel> Map(IEnumerable<SecureFileViewModel> secureModels)
        {
            return secureModels.Select(Map);
        }

        #endregion

        public SecureFileViewModel CreateViewModel()
        {
            return this._viewModelFactory.CreateSecureFileViewModel();
        }
    }
}