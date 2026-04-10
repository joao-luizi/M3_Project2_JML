using DalPro;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public interface IUtilizadoresRepository
    {
        public List<Utilizador> GetAll(string tag);
        public Utilizador GetById(long id, string tag);
        public long Insert(Utilizador u, string tag);
        public void Update(Utilizador u, string tag);
        public void Delete(long id, string tag);
    }
}
