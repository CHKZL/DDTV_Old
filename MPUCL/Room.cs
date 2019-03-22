using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPUCL
{
    public class Room
    {
        public class RoomBox
        {
            public List<RoomCadr> data { get; set; }
        }

        public class RoomCadr
        {
            public string Name { get; set; }
            public string RoomNumber { get; set; }
            public string Types { get; set; } = "bilibili";
            public bool status { get; set; } =false;
            public bool VideoStatus { get; set; } = false;

            [JsonProperty(PropertyName = "Ty")]
            private bool statusAlt1 { set { status = value;} }
        }

        /// <summary>
        /// 房间的动态数据
        /// </summary>
        public class RoomInfo
        {
            public string Name { get; set; }
            public string RoomNumber { get; set; }
            public string Text { get; set; }
            public string steam { get; set; }
            public bool status { get; set; }
            public bool Top { get; set; }

        }

        public class RecordVideo
        {
            public string guid { get; set; }
            public string RoomID { get; set; }
            public string Name { get; set; }
            public bool Status { get; set; }
            public string File { get; set; }
            public string StartTime { get; set; } = "";
            public string EndTime { get; set; } = "";
        }
    }
}
