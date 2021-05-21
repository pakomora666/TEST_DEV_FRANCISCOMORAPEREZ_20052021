using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TEST_DEV_FRANCISCOMORAPEREZ_20052021.Models
{
    public class PersonasFisicasModel
    {
        [Key]
        public int IdPersonaFisica { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaActualizacion { get; set; }
        [Required, MaxLength(50)]
        public string Nombre { get; set; }
        [Required, MaxLength(50)]
        public string ApellidoPaterno { get; set; }
        [Required, MaxLength(50)]
        public string ApellidoMaterno { get; set; }
        [Required, MaxLength(13)]
        public string RFC { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int UsuarioAgrega { get; set; }
        public Boolean Activo { get; set; }

    }
}
