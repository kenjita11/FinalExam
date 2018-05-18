using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AudioWatermark.Models
{
    public class GoogleDriveFiles
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string WebViewLink { get; set; }
        public long? Size { get; set; }

        public DateTime? CreatedTime { get; set; }
    }
}