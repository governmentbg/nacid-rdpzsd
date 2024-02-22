using System;

namespace Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser
{
    public class TxtValidationErrorCodeDto<TEnum>
        where TEnum : struct, IConvertible
    {
        public int ErrorCodeNumber { get; set; }
        public TEnum ErrorCode { get; set; }
    }
}
