using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTOBusiness.DTOs
{
    public record NucleoDTO(
        long Id,
        string Nome,
        string Local,
        string TipoDescricao
    );

    public record SaveNucleoDTO(
        string Nome,
        string Local,
        byte IdTipoNucleo
    );
}
