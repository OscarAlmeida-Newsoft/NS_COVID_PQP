using NS_COVID_Entities.Entities;
using NS_EncuestaCOVID.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NS_EncuestaCOVID.Models
{
    public class PersonaVM
    {
        public int Id { get; set; }

        [DisplayName("Tipo Documento:")]
        [Required(ErrorMessage = "Debe seleccionar tipo de documento")]
        public string TipoDocumento { get; set; }

        [DisplayName("Número Documento:")]
        [Required(ErrorMessage = "Debe ingresar el número de documento")]
        public string NumeroDocumento { get; set; }

        [DisplayName("Nombres:")]
        [Required(ErrorMessage = "Debe ingresar los nombres")]
        public string Nombres { get; set; }

        [DisplayName("Apellidos:")]
        [Required(ErrorMessage = "Debe ingresar los apellidos")]
        public string Apellidos { get; set; }

        [DisplayName("Sexo:")]
        [Required(ErrorMessage = "Debe seleccionar el sexo")]
        public string Sexo { get; set; }

        [DisplayName("Fecha de Nacimiento:")]
        [Required(ErrorMessage = "Debe ingresar la fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [TranslatorPhone(DataType.PhoneNumber, "Celular")]
        [DisplayName("Celular:")]
        [Required(ErrorMessage = "Debe ingresar el celular")]
        public string NumeroCelular { get; set; }

        [TranslatorEmail(DataType.EmailAddress, fieldNameTextIdentifier: "Correo Electrónico")]
        [DisplayName("Correo Electrónico:")]
        [Required(ErrorMessage = "Debe ingresar el correo electrónico")]
        public string CorreoElectronico { get; set; }

        [DisplayName("Tipo de vínculo")]
        [Required(ErrorMessage = "Debe seleccionar el vínculo que tiene con PQP")]
        public string Vinculo { get; set; }

        [RequiredIf("EmpleadoDirecto", false, "Debe ingresar la empresa")]
        public string Empresa { get; set; }

        public string Placa { get; set; }

        [DisplayName("Términos de uso")]
        [Required(ErrorMessage = "Debe aceptar los terminos de uso")]
        public bool AceptaTerminosUso { get; set; }
        public string FechaHoraCreacion { get; set; }

        //Listas de datos a cargar al usuario
        public SelectList TipoDocumentoList { get; set; }
        public SelectList Vinculos { get; set; }
        public List<Pregunta> cuestionario { get; set; }
        public List<DetalleRespuesta> respuestas { get; set; }
        public bool EmpleadoDirecto { get; set; }

        public string cadenaPregunta  { get; set; }
    }
}