using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNH.Domain.Exceptions
{
    public class CustomException : Exception
    {
        public enum Type
        {
            Unknown,

        }

        public string ShowException(Type type)
        {
            switch (type)
            {
                case Type.Unknown:
                    return "Lỗi không xác định!";
                    // Noted log error ...
            }
            return "Lỗi xử lý";
        }
    }
}
