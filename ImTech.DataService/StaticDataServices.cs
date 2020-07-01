using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImTech.Model;
using ImTech.DataBase;
using System.Data;

namespace ImTech.DataServices
{
    public class StaticDataServices
    {
        IDataBaseService dataBaseService;

        public StaticDataServices(IDataBaseService dataBaseService)
        {
            this.dataBaseService = dataBaseService;
            Start();
        }

        public void Start()
        {
            CityList = GetCityModelCollection();
            CountryList = GetCountryModelCollection();
            StateList = GetStateModelCollection();
            DegreeList = GetDegreeModelCollection();
            DeseaseList = GetDeseaseModelCollection();
            SpecializationList = GetSpecializationModelCollection();
            HospitalList = GetHospitalModelCollection();
        }

        #region Country Master

        private IEnumerable<Country> CountryList;

        Country MapCountry(DataRow row)
        {
            return new Country
            {
                Id = Convert.ToInt32(row["CountryID"]),
                CountryName = Convert.ToString(row["CountryName"]),
                IsDeleted = Convert.ToInt32(row["IsDeleted"]) == 0 ? false : true
            };
        }

        public IEnumerable<Country> GetCountry(Func<Country, bool> predicate)
        {
            return CountryList.Where(predicate);
        }

        public IEnumerable<Country> GetCountry()
        {
            return CountryList;
        }

        private IEnumerable<Country> GetCountryModelCollection()
        {

            IEnumerable<Country> country = null;
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Id", -1));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);

            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetCountryOrCountryList, DBCommandType.Procedure, param.ToArray());
            //int retValue = Convert.ToInt32(p_IsError.Value);
            //if (retValue >= (int)StoreProcStatusEnum.Success)
            //{
            if (dataSet.Tables[0] != null)
            {
                country = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapCountry(r));
            }
            //}
            if (country == null)
            {
                country = new List<Country>();
            }
            return country;
        }

        //private IEnumerable<Country> GetCountryModelCollection()
        //{
        //    var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetCountryOrCountryList, DBCommandType.Procedure);
        //    var country = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapCountry(r));
        //    return country;
        //}
        #endregion

        #region State Master
        private IEnumerable<State> StateList;

        State MapState(DataRow row)
        {
            return new State
            {
                Id = Convert.ToInt32(row["StateID"]),
                CountryId = Convert.ToInt32(row["CountryID"]),
                StateName = Convert.ToString(row["StateName"]),
                IsDeleted = Convert.ToInt32(row["IsDeleted"]) == 0 ? false : true
            };
        }

        public IEnumerable<State> GetState(Func<State, bool> predicate)
        {
            return StateList.Where(predicate);
        }

        public IEnumerable<State> GetState()
        {
            return StateList;
        }

        private IEnumerable<State> GetStateModelCollection()
        {
            IEnumerable<State> state = null;
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Id", -1));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetStateOrStateList, DBCommandType.Procedure, param.ToArray());
            //int retValue = Convert.ToInt32(p_IsError);
            //if (retValue >= (int)StoreProcStatusEnum.Success)
            //{
            if (dataSet.Tables[0] != null)
            {
                state = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapState(r));
            }
            //}
            if (state == null)
            {
                state = new List<State>();
            }
            return state;
        }

        #endregion

        #region City Master
        private IEnumerable<City> CityList;

        City MapCity(DataRow row)
        {
            return new City
            {
                Id = Convert.ToInt32(row["CityID"]),
                StateId = Convert.ToInt32(row["StateID"]),
                CityName = Convert.ToString(row["CityName"]),
                IsDeleted = Convert.ToInt32(row["IsDeleted"]) == 0 ? false : true
            };
        }

        public IEnumerable<City> GetCity(Func<City, bool> predicate)
        {
            return CityList.Where(predicate);
        }

        public IEnumerable<City> GetCity()
        {
            return CityList;
        }

        private IEnumerable<City> GetCityModelCollection()
        {
            IEnumerable<City> state = null;
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Id", -1));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetCityOrCityList, DBCommandType.Procedure, param.ToArray());
            //int retValue = Convert.ToInt32(p_IsError);
            //if (retValue >= (int)StoreProcStatusEnum.Success)
            //{
            //    if (dataSet.Tables[0] != null)
            //    {
            state = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapCity(r));
            //    }
            //}
            if (state == null)
            {
                state = new List<City>();
            }
            return state;
        }
        #endregion

        #region Degree Master
        private IEnumerable<DegreeModel> DegreeList;

        DegreeModel MapDegree(DataRow row)
        {
            return new DegreeModel
            {
                Id = Convert.ToInt32(row["DegreeID"]),
                DegreeName = Convert.ToString(row["DegreeName"]),
            };
        }

        public IEnumerable<DegreeModel> GetDegree(Func<DegreeModel, bool> predicate)
        {
            return DegreeList.Where(predicate);
        }

        public IEnumerable<DegreeModel> GetDegree()
        {
            return DegreeList;
        }

        public IEnumerable<DegreeModel> GetDegreeModelCollection()
        {
            IEnumerable<DegreeModel> degree = null;
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Id", -1));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetDegreeOrDegreeList, DBCommandType.Procedure);
            //int retValue = Convert.ToInt32(p_IsError);
            //if (retValue >= (int)StoreProcStatusEnum.Success)
            //{
            if (dataSet.Tables[0] != null)
            {
                degree = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapDegree(r));
            }
            //}
            if (degree == null)
            {
                degree = new List<DegreeModel>();
            }
            return degree;
        }
        #endregion

        #region Specialization Master
        private IEnumerable<SpecializationModel> SpecializationList;

        SpecializationModel MapSpecialization(DataRow row)
        {
            return new SpecializationModel
            {
                Id = Convert.ToInt32(row["SpecializationID"]),
                SpecializationName = Convert.ToString(row["SpecializationName"]),
            };
        }

        public IEnumerable<SpecializationModel> GetSpecialization(Func<SpecializationModel, bool> predicate)
        {
            return SpecializationList.Where(predicate);
        }

        public IEnumerable<SpecializationModel> GetSpecialization()
        {
            return SpecializationList;
        }

        public IEnumerable<SpecializationModel> GetSpecializationModelCollection()
        {
            IEnumerable<SpecializationModel> specialization = null;
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Id", -1));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetSpecializationOrSpecializationList, DBCommandType.Procedure);
            // int retValue = Convert.ToInt32(p_IsError);
            // if (retValue >= (int)StoreProcStatusEnum.Success)
            //{
            if (dataSet.Tables[0] != null)
            {
                specialization = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapSpecialization(r));
            }
            // }
            if (specialization == null)
            {
                specialization = new List<SpecializationModel>();
            }
            return specialization;
        }

        public IEnumerable<HospitalModel> GetHospitalorHospitalsListDetail()
        {
            IEnumerable<HospitalModel> hospitals = null;
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Id", -1));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetHospitalOrHospitalList, DBCommandType.Procedure);
            if (dataSet.Tables[0] != null)
            {
                hospitals = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapHospitalDetail(r));
            }
            if (hospitals == null)
            {
                hospitals = new List<HospitalModel>();
            }
            return hospitals;
        }

        public HospitalModel GetHospitalDetail(long id)
        {
            HospitalModel hospital = null;
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Id", id));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetHospitalOrHospitalList, DBCommandType.Procedure, param.ToArray());
            if (dataSet.Tables[0] != null)
            {
                hospital = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapHospitalDetail(r)).FirstOrDefault();
            }
            if (hospital == null)
            {
                hospital = new HospitalModel();
            }
            return hospital;
        }

        public HospitalModel SaveHospital(HospitalModel hModel)
        {
            List<Parameter> saveHParam = GetSaveHospitalParams(hModel);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            saveHParam.Add(p_IsError);
            saveHParam.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertUpdateHospital, DBCommandType.Procedure, saveHParam.ToArray<Parameter>());
            int retValue = Convert.ToInt32(r);
            if (retValue < (int)StoreProcStatusEnum.Success)
            {
                hModel.Success = false;
                hModel.ErrorMessage = p_ErrorMsg.ToString();
            }
            else
            {
                hModel.Success = true;
                hModel.LongId = retValue;
            }
            return hModel;
        }

        public DegreeModel SaveDegree(DegreeModel dModel)
        {
            List<Parameter> saveHParam = GetSaveDegreeParams(dModel);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            saveHParam.Add(p_IsError);
            saveHParam.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertUpdateDegree, DBCommandType.Procedure, saveHParam.ToArray<Parameter>());
            int retValue = Convert.ToInt32(r);
            if (retValue < (int)StoreProcStatusEnum.Success)
            {
                dModel.Success = false;
                dModel.ErrorMessage = p_ErrorMsg.ToString();
            }
            else
            {
                dModel.Success = true;
                dModel.Id = retValue;
            }
            return dModel;
        }

        public SpecializationModel SaveSpecialization(SpecializationModel sModel)
        {
            List<Parameter> saveHParam = GetSaveSpecializationParams(sModel);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            saveHParam.Add(p_IsError);
            saveHParam.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertUpdateSpecialization, DBCommandType.Procedure, saveHParam.ToArray<Parameter>());
            int retValue = Convert.ToInt32(r);
            if (retValue < (int)StoreProcStatusEnum.Success)
            {
                sModel.Success = false;
                sModel.ErrorMessage = p_ErrorMsg.ToString();
            }
            else
            {
                sModel.Success = true;
                sModel.Id = retValue;
            }
            return sModel;
        }

        public DeseaseModel SaveDesease(DeseaseModel dModel)
        {
            List<Parameter> saveHParam = GetSaveDeseaseParams(dModel);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            saveHParam.Add(p_IsError);
            saveHParam.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertUpdateDesease, DBCommandType.Procedure, saveHParam.ToArray<Parameter>());
            int retValue = Convert.ToInt32(r);
            if (retValue < (int)StoreProcStatusEnum.Success)
            {
                dModel.Success = false;
                dModel.ErrorMessage = p_ErrorMsg.ToString();
            }
            else
            {
                dModel.Success = true;
                dModel.Id = retValue;
            }
            return dModel;
        }




        #endregion

        #region Hospital Master
        private IEnumerable<HospitalModel> HospitalList;

        HospitalModel MapHospital(DataRow row)
        {
            return new HospitalModel
            {
                HospitalID = Convert.ToInt32(row["HospitalID"]),
                HospitalName = Convert.ToString(row["HospitalName"]),
            };
        }

        private HospitalModel MapHospitalDetail(DataRow row)
        {
            return new HospitalModel
            {
                LongId = Convert.ToInt64(row["HospitalID"]),
                HospitalCode = Convert.ToString(row["HospitalCode"]),
                HospitalName = Convert.ToString(row["HospitalName"]),
                CityId = row["HospitalCity"] != DBNull.Value ? Convert.ToInt64(row["HospitalCity"]) : 0,
                StateId = row["HospitalState"] != DBNull.Value ? Convert.ToInt64(row["HospitalState"]) : 0,
                CityName = Convert.ToString(row["CityName"]),
                StateName = Convert.ToString(row["StateName"]),
                HospitalAddress1 = Convert.ToString(row["HospitalAddress1"]),
                HospitalEmail = row["HospitalEmailID"] != DBNull.Value ? Convert.ToString(row["HospitalEmailID"]) : string.Empty,
                HospitalPhone1 = row["HospitalPhoneNumber1"] != DBNull.Value ? Convert.ToString(row["HospitalPhoneNumber1"]) : string.Empty,
                HospitalPhone2 = row["HospitalPhoneNumber1"] != DBNull.Value ? Convert.ToString(row["HospitalPhoneNumber2"]) : string.Empty,
                HospitalFax = row["HospitalFaxNumber"] != DBNull.Value ? Convert.ToString(row["HospitalFaxNumber"]) : string.Empty,
                IsDeleted = row["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(row["IsDeleted"]) : false,
            };
        }

        public IEnumerable<HospitalModel> GetHospital(Func<HospitalModel, bool> predicate)
        {
            return HospitalList.Where(predicate);
        }

        public IEnumerable<HospitalModel> GetHospital()
        {
            return HospitalList;
        }

        private IEnumerable<HospitalModel> GetHospitalModelCollection()
        {
            IEnumerable<HospitalModel> hospitals = null;
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Id", -1));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetHospitalOrHospitalList, DBCommandType.Procedure);
            // int retValue = Convert.ToInt32(p_IsError);
            // if (retValue >= (int)StoreProcStatusEnum.Success)
            //{
            if (dataSet.Tables[0] != null)
            {
                hospitals = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapHospital(r));
            }
            // }
            if (hospitals == null)
            {
                hospitals = new List<HospitalModel>();
            }
            return hospitals;
        }

        private List<Parameter> GetSaveHospitalParams(HospitalModel hospitalModel)
        {
            return new Parameter[]
            {
                new Parameter("@Id",  hospitalModel.LongId),
                new Parameter("@HospitalName", hospitalModel.HospitalName),
                new Parameter("@HospitalEmail", hospitalModel.HospitalEmail),
                new Parameter("@HospitalPhone1", hospitalModel.HospitalPhone1),
                new Parameter("@HospitalPhone2", hospitalModel.HospitalPhone2),
                new Parameter("@HospitalFax", hospitalModel.HospitalFax),
                new Parameter("@HospitalAddress1",hospitalModel.HospitalAddress1),
                new Parameter("@HospitalCity",hospitalModel.CityId),
                new Parameter("@HospitalState", hospitalModel.StateId),

                new Parameter("@TenantID", hospitalModel.TenantID),
                new Parameter("@CreatedBy", hospitalModel.CreatedBy),
                new Parameter("@CreatedByEntity", hospitalModel.CreatedByEntity),
                new Parameter("@UpdatedBy", hospitalModel.ModifiedBy),
                new Parameter("@UpdatedByEntity", hospitalModel.ModifiedByEntity),

                new Parameter("@HospitalCode", hospitalModel.HospitalCode),
            }.ToList<Parameter>();
        }

        private List<Parameter> GetSaveDegreeParams(DegreeModel degreeModel)
        {
            return new Parameter[]
            {
                new Parameter("@Id",  degreeModel.Id),
                new Parameter("@DegreeName", degreeModel.DegreeName),
                new Parameter("@TenantID", degreeModel.TenantID),
                new Parameter("@CreatedBy", degreeModel.CreatedBy),
                new Parameter("@CreatedByEntity", degreeModel.CreatedByEntity),
                new Parameter("@UpdatedBy", degreeModel.ModifiedBy),
                new Parameter("@UpdatedByEntity", degreeModel.ModifiedByEntity)
            }.ToList<Parameter>();
        }

        private List<Parameter> GetSaveSpecializationParams(SpecializationModel specializationModel)
        {
            return new Parameter[]
            {
                new Parameter("@Id",  specializationModel.Id),
                new Parameter("@SpecializationName", specializationModel.SpecializationName),
                new Parameter("@TenantID", specializationModel.TenantID),
                new Parameter("@CreatedBy", specializationModel.CreatedBy),
                new Parameter("@CreatedByEntity", specializationModel.CreatedByEntity),
                new Parameter("@UpdatedBy", specializationModel.ModifiedBy),
                new Parameter("@UpdatedByEntity", specializationModel.ModifiedByEntity)
            }.ToList<Parameter>();
        }

        private List<Parameter> GetSaveDeseaseParams(DeseaseModel deseaseModel)
        {
            return new Parameter[]
            {
                new Parameter("@Id",  deseaseModel.Id),
                new Parameter("@DeseaseName", deseaseModel.DeseaseName),
                new Parameter("@TenantID", deseaseModel.TenantID),
                new Parameter("@CreatedBy", deseaseModel.CreatedBy),
                new Parameter("@CreatedByEntity", deseaseModel.CreatedByEntity),
                new Parameter("@UpdatedBy", deseaseModel.ModifiedBy),
                new Parameter("@UpdatedByEntity", deseaseModel.ModifiedByEntity)
            }.ToList<Parameter>();
        }
        #endregion

        #region Desease Master
        private IEnumerable<DeseaseModel> DeseaseList;

        DeseaseModel MapDesease(DataRow row)
        {
            return new DeseaseModel
            {
                Id = Convert.ToInt32(row["DeseaseID"]),
                DeseaseName = Convert.ToString(row["DeseaseName"]),
            };
        }

        public IEnumerable<DeseaseModel> GetDesease(Func<DeseaseModel, bool> predicate)
        {
            return DeseaseList.Where(predicate);
        }

        public IEnumerable<DeseaseModel> GetDesease()
        {
            return DeseaseList;
        }

        public IEnumerable<DeseaseModel> GetDeseaseModelCollection()
        {
            IEnumerable<DeseaseModel> decease = null;
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@Id", -1));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 100);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetDeseaseOrDeseaseList, DBCommandType.Procedure);
            //int retValue = Convert.ToInt32(p_IsError);
            //if (retValue >= (int)StoreProcStatusEnum.Success)
            //{
            if (dataSet.Tables[0] != null)
            {
                decease = dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => MapDesease(r));
            }
            //}
            if (decease == null)
            {
                decease = new List<DeseaseModel>();
            }
            return decease;
        }
        #endregion
    }
}
