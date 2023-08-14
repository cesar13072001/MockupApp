using Microsoft.Ajax.Utilities;
using MockupApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MockupApp.DAO
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                resultado = 0;
            }
            
            return resultado;

        }


        public int verificarCorreo(string email)
        {
            int resultado = 0;
            try
            {
                var consulta = from u in db.Usuario.Where(u => u.correo == email)
                                select u;
                resultado = consulta.Count(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return resultado;
        }


        public sp_LoginUsuario_Result logeo(string email, string password)
        {
            sp_LoginUsuario_Result usuario = new sp_LoginUsuario_Result() ;
           

            try
            {
                var consulta = db.sp_LoginUsuario(email, password).ToList();
                if (consulta.Count() == 0) return usuario;
                foreach (var con in consulta)
                {
                    usuario = con;

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return usuario;
        }

    }
}