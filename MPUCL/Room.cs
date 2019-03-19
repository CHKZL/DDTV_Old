using Newtonsoft.Json;
using System.Collections.Generic;

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
            public string Types { get; set; }
            public bool status { get; set; }
            public bool VideoStatus { get; set; }

            [JsonProperty(PropertyName = "Ty")]
            private bool statusAlt1 { set { status = value; } }
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
    }
}
