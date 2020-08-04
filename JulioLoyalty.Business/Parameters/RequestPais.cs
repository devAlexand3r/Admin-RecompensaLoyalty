using JulioLoyalty.Business.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulioLoyalty.Business.Parameters
{
    public class RequestPais : IValidatableObject
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Clave")]
        public string clave { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Descripcion Larga")]
        public string descripcion_larga { get; set; }

        [StringLength(100)]
        [Display(Name = "Correo electronico")]
        public string correo_electronico { get; set; }

        [StringLength(50)]
        [Display(Name = "Remitente")]
        public string remitente { get; set; }

        [StringLength(100)]
        [Display(Name = "Servidor POP")]
        public string servidor_pop { get; set; }

        [StringLength(100)]
        [Display(Name = "Servidor STMP")]
        public string servidor_smtp { get; set; }

        [StringLength(50)]
        [Display(Name = "Usuario Correo")]
        public string usuario_correo { get; set; }

        [StringLength(20)]
        [Display(Name = "Contraseña Correo")]
        public string password_correo { get; set; }

        [StringLength(50)]
        [Display(Name = "Prefijo RMS")]
        public string prefijo_rms { get; set; }

        [StringLength(20)]
        [Display(Name = "Usuario RMS")]
        public string usuario_rms { get; set; }

        [StringLength(20)]
        [Display(Name = "Password RMS")]
        public string password_rms { get; set; }

        [Display(Name = "Valor Punto")]
        public double? valor_punto { get; set; }

        [StringLength(250)]
        [Display(Name = "URL Programa")]
        public string url_programa { get; set; }

        [StringLength(5)]
        [Display(Name = "Clave Carge")]
        public string clave_carga { get; set; }

        [StringLength(250)]
        [Display(Name = "URL Logo")]
        public string url_logo { get; set; }

        [Display(Name = "Tipo Envio Digital")]
        public byte? tipo_envio_digital { get; set; }

        [Display(Name = "Tipo Envio Recarga")]
        public byte? tipo_envio_recarga { get; set; }

        [Display(Name = "Tipo Envio Fisico")]
        public byte? tipo_envio_fisico { get; set; }

        [Display(Name = "Banner Carousel")]
        public string banner_carousel { get; set; }

        [StringLength(255)]
        [Display(Name = "Tema")]
        public string theme { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (!string.IsNullOrWhiteSpace(this.correo_electronico))
            {
                Validator.TryValidateProperty(this.correo_electronico, new ValidationContext(this, null, null) { MemberName = nameof(correo_electronico) }, results);
                if (!RegexUtilities.IsValidEmail(this.correo_electronico))
                {
                    results.Add(new ValidationResult("Campo Invalido.", new List<string>() { nameof(correo_electronico) }));
                }
            }

            if (!string.IsNullOrWhiteSpace(this.usuario_correo))
            {
                Validator.TryValidateProperty(this.usuario_correo, new ValidationContext(this, null, null) { MemberName = nameof(usuario_correo) }, results);
                if (!RegexUtilities.IsValidEmail(this.usuario_correo))
                {
                    results.Add(new ValidationResult("Campo Invalido.", new List<string>() { nameof(usuario_correo) }));
                }
            }
            return results;
        }
    }
}
