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
    public class SecureFileService
    {
        IDataBaseService dataBaseService;
        FileSystem fileSystem;
        public SecureFileService(IDataBaseService databaseService, FileSystem fileSystem)
        {
            dataBaseService = databaseService;
            this.fileSystem = fileSystem;
        }

        SecureFileModel Map(DataRow row)
        {

            return new SecureFileModel()
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

        IEnumerable<SecureFileModel> Map(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows.Cast<DataRow>())
            {
                yield return Map(row);
            }
        }

        #region Methods

        public SecureFileModel GetSecureFileDetails(SecureFileModel model)
        {
            var fileParam = new Parameter("@Id", model.Id);
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);

            var dataSet = dataBaseService.GetDataSet(StoredProcedures.GetSecureFileDetail, DBCommandType.Procedure, fileParam);
            return dataSet.Tables[0].Rows.Cast<DataRow>().Select(r => Map(r)).FirstOrDefault();

        }

        public string CheckSecureFileAccess(Int64 fileId, Int64 userId, int userEntity)
        {
            string ErrorMessage = string.Empty;
            List<Parameter> param = new List<Parameter>();
            param.Add(new Parameter("@UserID", userId));
            param.Add(new Parameter("@FileId", fileId));
            param.Add(new Parameter("@UserEntity", userEntity));
            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);
            param.Add(p_IsError);
            param.Add(p_ErrorMsg);
            var r = dataBaseService.ExecuteScalar(StoredProcedures.CheckSecureFileAccess, DBCommandType.Procedure, param.ToArray());
            int Id = Convert.ToInt16(r);
            if (Id < 0)
            {
                ErrorMessage = "Access Denied to The File";
            }
            return ErrorMessage;
        }
        public SecureFileModel UploadFileToDB(SecureFileModel model)
        {
            var p_FileName = new Parameter("@FileName", model.FileName);
            var p_FileExtension = new Parameter("@FileExtension", model.FileExtension);
            var p_FileDocumentType = new Parameter("@FileDocumentType", model.FileDocType);
            var p_FilePath = new Parameter("@FilePath", model.FileFullPath);
            var p_ActualFileName = new Parameter("@ActualFileName", model.ActualFileName);
            var p_TenantID = new Parameter("@TenantID", model.TenantID);
            var p_CreatedBy = new Parameter("@CreatedBy", model.CreatedBy);
            var p_CreatedByEntity = new Parameter("@CreatedByEntity", model.CreatedByEntity);

            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);

            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertUpdateDeleteSecureFile
                    , DBCommandType.Procedure
                    , p_FileName
                    , p_FileExtension
                    , p_FileDocumentType
                    , p_FilePath
                    , p_ActualFileName
                    , p_TenantID
                    , p_CreatedBy
                    , p_CreatedByEntity
                    , p_IsError
                    , p_ErrorMsg
                   );

            return PrepareReturnModel(Convert.ToInt32(r), Convert.ToString(""), model);
        }
        public byte[] DownloadFile(SecureFileModel fileModel)
        {

            return this.fileSystem.Read(fileModel).Contents;
        }

        public SecureFileModel DeleteFile(SecureFileModel model)
        {
            var fileParam = new Parameter("@FileID", model.Id);
            var p_OpnFlag = new Parameter("@OpnFlag", 3);

            Parameter p_IsError = new Parameter("@IsError", DBNull.Value, ParameterDirection.Output, DbType.Int16, 1);
            Parameter p_ErrorMsg = new Parameter("@ErrorMsg", DBNull.Value, ParameterDirection.Output, DbType.String, 16);

            var r = dataBaseService.ExecuteScalar(StoredProcedures.InsertUpdateDeleteSecureFile, DBCommandType.Procedure, fileParam);

            return PrepareReturnModel(Convert.ToInt32(r), Convert.ToString(p_ErrorMsg), model);
        }


        public SecureFileModel UploadToFileSystem(SecureFileModel fileModel, int userId, System.IO.Stream stream)
        {
            fileModel.FileFullPath = this.fileSystem.GetFolderPath(fileModel);
            fileModel.FileName = this.fileSystem.GetGeneratedFileName(fileModel, userId);
            fileModel.ContentStream = stream;

            fileSystem.Write(fileModel);

            return fileModel;
        }

        private SecureFileModel PrepareReturnModel(int fileId, string p_ErrorMsg, SecureFileModel model)
        {
            int retValue = fileId;

            if (retValue < (int)StoreProcStatusEnum.Success)
            {
                model.Success = false;
                model.ErrorMessage = p_ErrorMsg;
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
