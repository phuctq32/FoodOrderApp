using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderApp.Models
{
    class Message
    {
        private static Message _ins;
        public static Message Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new Message();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        public CHATMESSAGEEntities ms { get; set; }
        private Message()
        {
            ms = new CHATMESSAGEEntities();
        }
    }
}
