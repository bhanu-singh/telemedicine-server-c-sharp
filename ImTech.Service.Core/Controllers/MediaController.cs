using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ImTech.DataServices;
using ImTech.Service.ViewModels;
using ImTech.Service.Mapper;
using System.Web;
using System.Threading.Tasks;
using ImTech.Model;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace ImTech.Service.Controllers
{
    public class MediaController : ApiController
    {
        private readonly StaticDataServices _staticDataServices;
        private DataServices.DataServices _dataServices;
        private ILogger _logger;
        private SecureFileMapper _secureMapper;
        private NonSecureFileMapper _nonSecureMapper;
        public MediaController(StaticDataServices staticDataServices, DataServices.DataServices dataServices, ILogger logger, SecureFileMapper secureMapper, NonSecureFileMapper nonSecureMapper)
        {
            _staticDataServices = staticDataServices;
            _dataServices = dataServices;
            _logger = logger;
            _secureMapper = secureMapper;
            _nonSecureMapper = nonSecureMapper;
        }

        [HttpGet]
        public HttpResponseMessage GetNonSecureFiles(int? id)
        {

            if (id.HasValue)
            {
                try
                {
                    if (id.Value > 0)
                    {
                        NonSecureFileModel fileModel = new NonSecureFileModel() { Id = id.Value };
                        fileModel = this._dataServices.NonSecureFileService.GetSecureFileDetails(fileModel);
                        byte[] filedata = new byte[1]; string contentType = "";
                        if (fileModel != null)
                        {
                            filedata = this._dataServices.NonSecureFileService.DownloadFile(fileModel);
                            contentType = MimeMapping.GetMimeMapping(fileModel.FileFullPath + fileModel.FileName);

                            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                            var stream = new MemoryStream(filedata);
                            result.Content = new StreamContent(stream);
                            result.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                            {
                                FileName = fileModel.FileName,
                            };
                            return result;
                        }
                    }
                    else
                    {
                        HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                        var stream = new FileStream(HttpContext.Current.Server.MapPath("~/TemFile/download.jpeg"), FileMode.OpenOrCreate);
                        result.Content = new StreamContent(stream);
                        result.Content.Headers.ContentType =
                            new MediaTypeHeaderValue("application/octet-stream");
                        result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            FileName = "doctor.png",

                        };
                        return result;
                    }
                }
                catch (Exception e)
                {
                    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                    var stream = new FileStream(HttpContext.Current.Server.MapPath("~/TemFile/download.jpeg"), FileMode.OpenOrCreate);
                    result.Content = new StreamContent(stream);
                    result.Content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "download.jpeg",

                    };
                    return result;
                }
            }
            HttpResponseMessage result1 = new HttpResponseMessage(HttpStatusCode.BadRequest);
            return result1;
        }


        #region Secure

        [HttpPost]
        public HttpResponseMessage GetSecureFileDetails(RequestCarrier requestCarrier) //currently return void need to change afterwards
        {
            if (requestCarrier != null && requestCarrier.TanentId != 0 && requestCarrier.From != string.Empty)
            {
                if (requestCarrier.PayLoad != null)
                {
                    var secureModel = this._secureMapper.Map(WebCommon.Instance.GetObject<SecureFileViewModel>(requestCarrier.PayLoad));
                    secureModel.CreatedBy = (int)requestCarrier.UserId.Value;
                    secureModel.CreatedByEntity = Convert.ToInt32(requestCarrier.From);
                    secureModel.TenantID = requestCarrier.TanentId;
                    string hasAccess = this._dataServices.SecureFileService.CheckSecureFileAccess(secureModel.Id, secureModel.CreatedBy, secureModel.CreatedByEntity);
                    if (string.IsNullOrEmpty(hasAccess))
                    {
                        if (secureModel.Id > 0)
                        {
                            SecureFileModel fileModel = new SecureFileModel() { Id = secureModel.Id };
                            fileModel = this._dataServices.SecureFileService.GetSecureFileDetails(fileModel);
                            byte[] filedata = new byte[1]; string contentType = "";
                            if (fileModel != null)
                            {
                                filedata = this._dataServices.SecureFileService.DownloadFile(fileModel);
                                contentType = MimeMapping.GetMimeMapping(fileModel.FileFullPath + fileModel.FileName);

                                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                                var stream = new MemoryStream(filedata);
                                result.Content = new StreamContent(stream);
                                result.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                                {
                                    FileName = fileModel.FileName,
                                };
                                return result;
                            }
                        }
                    }
                    else
                    {
                        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    }
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public IHttpActionResult NonSecureUpload()
        {
            ResponseCarrier response;
            string fileIds = string.Empty;
            bool finalsucc = true;
            var httpRequest = HttpContext.Current.Request;
            NonSecureFileModel fileModel;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string f in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[f];
                    fileModel = new NonSecureFileModel();
                    fileModel.ActualFileName = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName);
                    fileModel.FileDocType = "nonsecure";
                    fileModel.FileExtension = System.IO.Path.GetExtension(postedFile.FileName);

                    fileModel = this._dataServices.NonSecureFileService.UploadToFileSystem(fileModel, -1, postedFile.InputStream);
                    fileModel.IsDeleted = false;


                    fileModel = this._dataServices.NonSecureFileService.UploadFileToDB(fileModel);
                    if (fileModel.Success)
                    {
                        fileIds += fileModel.Id + "|";
                    }
                    else
                    {
                        finalsucc = false;
                        fileIds = fileModel.ErrorMessage;
                    }

                }
                if (finalsucc)
                {
                    response = new ResponseCarrier()
                    {
                        PayLoad = fileIds.Remove(fileIds.Length - 1, 1),
                        Status = true,
                        ErrorMessage = string.Empty
                    };
                }
                else
                {
                    response = new ResponseCarrier()
                    {
                        PayLoad = null,
                        Status = false,
                        ErrorMessage = string.Empty
                    };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "No files provided." };
            }

            return Json(response);

        }
        [HttpPost]
        public IHttpActionResult SecureUpload()
        {
            ResponseCarrier response;
            string fileIds = string.Empty;
            bool finalsucc = true;
            var httpRequest = HttpContext.Current.Request;
            SecureFileModel fileModel;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string f in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[f];
                    fileModel = new SecureFileModel();
                    fileModel.ActualFileName = System.IO.Path.GetFileNameWithoutExtension(postedFile.FileName);
                    fileModel.FileDocType = "secure";
                    fileModel.FileExtension = System.IO.Path.GetExtension(postedFile.FileName);

                    fileModel = this._dataServices.SecureFileService.UploadToFileSystem(fileModel, -1, postedFile.InputStream);
                    fileModel.IsDeleted = false;


                    fileModel = this._dataServices.SecureFileService.UploadFileToDB(fileModel);
                    if (fileModel.Success)
                    {
                        fileIds += fileModel.Id + "|";
                    }
                    else
                    {
                        finalsucc = false;
                        fileIds = fileModel.ErrorMessage;
                    }

                }
                if (finalsucc)
                {
                    response = new ResponseCarrier()
                    {
                        PayLoad = fileIds.Remove(fileIds.Length - 1, 1),
                        Status = true,
                        ErrorMessage = string.Empty
                    };
                }
                else
                {
                    response = new ResponseCarrier()
                    {
                        PayLoad = null,
                        Status = false,
                        ErrorMessage = string.Empty
                    };
                }
            }
            else
            {
                response = new ResponseCarrier() { Status = false, PayLoad = null, ErrorMessage = "No files provided." };
            }

            return Json(response);

        }


    }
    #endregion


}

