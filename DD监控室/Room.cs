using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD监控室
{
    class Room
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
