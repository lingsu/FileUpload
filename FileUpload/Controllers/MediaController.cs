using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web.Http;
using Lyu.Core.Configuration;
using Lyu.Core.IO;
using Lyu.Web.Models;
using Lyu.Web.Models.ContentEditing;
using Lyu.Web.WebApi;

namespace FileUpload.Controllers
{
    public class MediaController : ApiController
    {
        public async Task<HttpResponseMessage> PostAddFile()
        {
            //检查该请求是否含有multipart/form-data
            if (Request.Content.IsMimeMultipartContent() == false)
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = IOHelper.MapPath("~/App_Data/TEMP/FileUploads");
            //ensure it exists
            Directory.CreateDirectory(root);
            var provider = new MultipartFormDataStreamProvider(root);

            var result = await Request.Content.ReadAsMultipartAsync(provider);

            //must have a file
            if (result.FileData.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            //get the string json from the request
            int parentId;
            if (int.TryParse(result.FormData["currentFolder"], out parentId) == false)
            {
                return Request.CreateValidationErrorResponse("The request was not formatted correctly, the currentFolder is not an integer");
            }

            var tempFiles = new PostedFiles();
            //get the files
            foreach (var file in result.FileData)
            {
                var fileName = file.Headers.ContentDisposition.FileName.Trim(new[] { '\"' });
                var ext = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();

                if (UmbracoConfig.For.UmbracoSettings().Content.DisallowedUploadFiles.Contains(ext) == false)
                {

                }
            }
        }

        /// <summary>
        /// This is used for the response of PostAddFile so that we can analyze the response in a filter and remove the 
        /// temporary files that were created.
        /// </summary>
        [DataContract]
        private class PostedFiles : IHaveUploadedFiles, INotificationModel
        {
            public PostedFiles()
            {
                UploadedFiles = new List<ContentItemFile>();
                Notifications = new List<Notification>();
            }
            public List<ContentItemFile> UploadedFiles { get; private set; }

            [DataMember(Name = "notifications")]
            public List<Notification> Notifications { get; private set; }
        }
    }
}
