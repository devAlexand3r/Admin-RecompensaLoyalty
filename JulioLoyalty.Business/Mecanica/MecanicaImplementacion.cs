using JulioLoyalty.Entities.Mecanica;
using JulioLoyalty.Model;
using JulioLoyalty.Model.EntitiesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Mecanica
{
    public class MecanicaImplementacion
    {
        public List<MecanicaNivel> MecanicaNivel()
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var resultados = db.nivel.Select(s => new MecanicaNivel
                {
                    Id = s.id,
                    Nombre = s.descripcion,
                    Porcentaje = s.porcentaje_acumulacion,
                    MontoMinimo = s.valor_nivel_inicial,
                    MontoMaximo = s.valor_nivel_final
                }).ToList();

                return resultados;
            }
        }

        public string ActualizarMecanicaNivel(MecanicaNivel mecanica)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var nivel = db.nivel.Find(mecanica.Id);
                if (nivel == null)
                    throw new Exception("El nivel no existe");

                nivel.descripcion = mecanica.Nombre;
                nivel.porcentaje_acumulacion = mecanica.Porcentaje;
                nivel.valor_nivel_inicial = mecanica.MontoMinimo;
                nivel.valor_nivel_final = mecanica.MontoMaximo;
                db.Entry(nivel).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return "Los datos se actualizaron correctamente";
            }
        }

        public string AgregarMecanicaNivel(MecanicaNivel mecanica)
        {
            using (DbContextJulio db = new DbContextJulio())
            {
                var nivel = new nivel()
                {
                    descripcion = mecanica.Nombre,
                    porcentaje_acumulacion = mecanica.Porcentaje,
                    valor_nivel_inicial = mecanica.MontoMinimo,
                    valor_nivel_final = mecanica.MontoMaximo
                };
                db.nivel.Add(nivel);
                db.SaveChanges();

                return "Los datos se agregaron correctamente";
            }
        }
    }
}
