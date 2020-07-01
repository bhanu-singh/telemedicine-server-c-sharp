using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

using ImTech.DataBase;
using ImTech.DataServices;
using ImTech.Model;

namespace ImTech.DataServices
{
    public class NonSecureFileService
    {
        IDataBaseService dataBaseService;
        FileSystem fileSystem;
        public NonSecureFileService(IDataBaseService databaseService, FileSystem fileServices)
        {
            dataBaseService = databaseService;
            this.fileSystem = fileServices;
        }

        NonSecureFileModel Map(DataRow row)
        {

            return new NonSecureFileModel()
            {
                Id = Convert.ToInt32(row["FileId"]),
                FileName = Convert.ToString(row["FileName"]),
                FileExtension = Convert.ToString(row["FileExtension"]),
                FileDocType = Convert.ToString(row["FileDocumentType"]),
                FileFullPath = Convert.ToString(row["FilePath"]),
                ActualFileName = Convert.ToString(row["ActualFileName"]),
                EntityType = Convert.ToString(row["EntityType"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                ModifiedOn = new InternalDateTime(Convert.ToInt64(row["CreatedOn"]))
            };
        }

        IEnumerable<NonSecureFileModel> Map(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows.Cast<DataRow>())
            {
                yield return Map(row);
            }
        }

        #region Methods

        public NonSecureFileModel GetSecureFileDetails(NonSecureFileModel model)
        {
            var fileParam = new Parameter("@Id", model.Id);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);

            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetNonSecureFileDetail, DBCommandType.Procedure, fileParam, p_IsError, p_ErrorMsg);
            return dataSet.Tables.Count > 0 ? dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => Map(r)).FirstOrDefault() : new NonSecureFileModel();

        }

        public NonSecureFileModel UploadFileToDB(NonSecureFileModel model)
        {
            var p_FileName = new Parameter("@FileName", model.FileName);
            var p_FileExtension = new Parameter("@FileExtension", model.FileExtension);
            var p_FileDocumentType = new Parameter("@FileDocumentType", model.FileDocType);
            var p_FilePath = new Parameter("@FilePath", model.FileFullPath);
            var p_ActualFileName = new Parameter("@ActualFileName", model.ActualFileName);
            var p_IsDeleted = new Parameter("@IsDeleted", model.IsDeleted);
            var p_TenantID = new Parameter("@TenantID", model.TenantID);
            var p_CreatedBy = new Parameter("@CreatedBy", model.CreatedBy);
            var p_CreatedByEntity = new Parameter("@CreatedByEntity", model.CreatedByEntity);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);

            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertUpdateDeleteNonSecureFile
                    , DBCommandType.Procedure
                    , p_FileName
                    , p_FileExtension
                    , p_FileDocumentType
                    , p_FilePath
                    , p_ActualFileName
                    , p_IsDeleted
                    , p_TenantID
                    , p_CreatedBy
                    , p_CreatedByEntity
                    , p_IsError
                    , p_ErrorMsg
                   );

            return PrepareReturnModel(r, model);
        }


        public NonSecureFileModel DeleteFile(NonSecureFileModel model)
        {
            var fileParam = new Parameter("@FileID", model.Id);
            var p_OpnFlag = new Parameter("@OpnFlag", 3);

            var p_IsError = new Parameter("@IsError", ParameterDirection.Output);
            var p_ErrorMsg = new Parameter("@ErrorMsg", ParameterDirection.Output);

            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertUpdateDeleteNonSecureFile, DBCommandType.Procedure, fileParam);

            return PrepareReturnModel(r, model);
        }


        public NonSecureFileModel UploadToFileSystem(NonSecureFileModel fileModel, int userId, System.IO.Stream stream)
        {
            fileModel.FileFullPath = this.fileSystem.GetFolderPath(fileModel);
            fileModel.ContentStream = stream;
            fileModel.FileName = this.fileSystem.GetGeneratedFileName(fileModel, userId);
            fileSystem.Write(fileModel);

            return fileModel;
        }
        public byte[] DownloadFile(NonSecureFileModel fileModel)
        {

            return this.fileSystem.Read(fileModel).Contents;
        }


        private NonSecureFileModel PrepareReturnModel(object r, NonSecureFileModel model)
        {
            int retValue = Convert.ToInt32(r);

            if (retValue < (int)StoreProcStatusEnum.Success)
            {
                model.Success = false;
                model.ErrorMessage = "There was an error occured while reading file."; ;
            }
            else
            {
                model.Success = true;
                model.Id = retValue;
            }
            return model;
        }

        #endregion
    }
}
