using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace DAOLayer
{
    public class AuthDAO
    {
        readonly mockupEntities db = new mockupEntities();


        public int guardarUsuario(Usuario usu)
        {
            int resultado = 0;
            try
            {
               db.Usuario.Add(usu);
               resultado = db.SaveChanges();
            
            }catch (Exception ex)
            {
                resultado = 0;
            }
            return resultado;

        }
    }
}
