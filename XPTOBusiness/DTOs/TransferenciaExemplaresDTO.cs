using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTOBusiness.DTOs
{
    public record TransferenciaExemplaresDTO(
        List<long> IdsExemplares,
        int IdNucleoDestino
    );
}
