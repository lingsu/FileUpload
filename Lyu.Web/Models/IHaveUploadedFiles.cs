using System.Collections.Generic;
using Lyu.Web.Models.ContentEditing;

namespace Lyu.Web.Models
{
    public interface IHaveUploadedFiles
    {
        List<ContentItemFile> UploadedFiles { get; }
    }
}