using System;
using System.Collections.Generic;

namespace Rdpzsd.Models.Dtos.RdpzsdImports.TxtParser
{
    public class LineDto<TEnum>
        where TEnum : struct, IConvertible
    {
        public int RowIndex { get; set; }
        public List<LineColumnDto> Columns { get; set; } = new List<LineColumnDto>();
        public List<TEnum> ErrorCodes { get; set; } = new List<TEnum>();

        public LineDtoType LineType { get; set; } = LineDtoType.NaturalPerson;
    }

    public enum LineDtoType
    {
        NaturalPerson = 1,
        Speciality = 2,
        DoctoralProgramme = 3
    }
}
