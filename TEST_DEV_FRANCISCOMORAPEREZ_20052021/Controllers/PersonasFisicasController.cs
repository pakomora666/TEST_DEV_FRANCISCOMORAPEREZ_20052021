using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TEST_DEV_FRANCISCOMORAPEREZ_20052021.Data;
using TEST_DEV_FRANCISCOMORAPEREZ_20052021.Models;

namespace TEST_DEV_FRANCISCOMORAPEREZ_20052021.Controllers
{
    public class PersonasFisicasController : Controller
    {
        private readonly IConfiguration _configuration;
        public PersonasFisicasController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: PersonasFisicas
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Conn")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "select * from Tb_PersonasFisicas";
                sqlDa.SelectCommand = cmd;
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }



        // GET: PersonasFisicas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PersonasFisicas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("IdPersonaFisica,Nombre,ApellidoPaterno,ApellidoMaterno,RFC,FechaNacimiento,UsuarioAgrega")] PersonasFisicasModel personasFisicasModel)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Conn")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("sp_AgregarPersonaFisica", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("Nombre", personasFisicasModel.Nombre);
                    sqlCmd.Parameters.AddWithValue("ApellidoPaterno", personasFisicasModel.ApellidoPaterno);
                    sqlCmd.Parameters.AddWithValue("ApellidoMaterno", personasFisicasModel.ApellidoMaterno);
                    sqlCmd.Parameters.AddWithValue("RFC", personasFisicasModel.RFC);
                    sqlCmd.Parameters.AddWithValue("FechaNacimiento", personasFisicasModel.FechaNacimiento);
                    sqlCmd.Parameters.AddWithValue("UsuarioAgrega", personasFisicasModel.UsuarioAgrega);
                    sqlCmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(personasFisicasModel);
        }

        // GET: PersonasFisicas/Edit/5
        public IActionResult Edit(int? id)
        {
            PersonasFisicasModel personasFisicasModel = new PersonasFisicasModel();
            if (id > 0)
                personasFisicasModel = FetchPersonaFisicaById(id);

            return View(personasFisicasModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("IdPersonaFisica,Nombre,ApellidoPaterno,ApellidoMaterno,RFC,FechaNacimiento,UsuarioAgrega")] PersonasFisicasModel personasFisicasModel)
        {
            if (id != personasFisicasModel.IdPersonaFisica)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Conn")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("sp_ActualizarPersonaFisica", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("IdPersonaFisica", personasFisicasModel.IdPersonaFisica);
                    sqlCmd.Parameters.AddWithValue("Nombre", personasFisicasModel.Nombre);
                    sqlCmd.Parameters.AddWithValue("ApellidoPaterno", personasFisicasModel.ApellidoPaterno);
                    sqlCmd.Parameters.AddWithValue("ApellidoMaterno", personasFisicasModel.ApellidoMaterno);
                    sqlCmd.Parameters.AddWithValue("RFC", personasFisicasModel.RFC);
                    sqlCmd.Parameters.AddWithValue("FechaNacimiento", personasFisicasModel.FechaNacimiento);
                    sqlCmd.Parameters.AddWithValue("UsuarioAgrega", personasFisicasModel.UsuarioAgrega);
                    sqlCmd.ExecuteNonQuery();

                }
                return RedirectToAction(nameof(Index));
            }
            return View(personasFisicasModel);
        }

        // GET: PersonasFisicas/Delete/5
        public IActionResult Delete(int? id)
        {
            PersonasFisicasModel personasFisicasModel = FetchPersonaFisicaById(id);
            return View(personasFisicasModel);

        }

        // POST: PersonasFisicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Conn")))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("sp_EliminarPersonaFisica", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("IdPersonaFisica", id);
                sqlCmd.ExecuteNonQuery();

            }
            return RedirectToAction(nameof(Index));

        }

        [NonAction]
        public PersonasFisicasModel FetchPersonaFisicaById(int? id)
        {
            PersonasFisicasModel personasFisicasModel = new PersonasFisicasModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Conn")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "select * from Tb_PersonasFisicas where IdPersonaFisica = " + id;
                sqlDa.SelectCommand = cmd;
                sqlDa.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    personasFisicasModel.IdPersonaFisica = Convert.ToInt32(dtbl.Rows[0]["IdPersonaFisica"].ToString());
                    personasFisicasModel.FechaRegistro = Convert.ToDateTime(dtbl.Rows[0]["FechaRegistro"].ToString());
                    personasFisicasModel.FechaActualizacion = dtbl.Rows[0]["FechaActualizacion"].ToString() != null ? DateTime.UtcNow : Convert.ToDateTime(dtbl.Rows[0]["FechaActualizacion"].ToString());
                    personasFisicasModel.Nombre = dtbl.Rows[0]["Nombre"].ToString();
                    personasFisicasModel.ApellidoPaterno = dtbl.Rows[0]["ApellidoPaterno"].ToString();
                    personasFisicasModel.ApellidoMaterno = dtbl.Rows[0]["ApellidoMaterno"].ToString();
                    personasFisicasModel.RFC = dtbl.Rows[0]["RFC"].ToString();
                    personasFisicasModel.FechaNacimiento = Convert.ToDateTime(dtbl.Rows[0]["FechaNacimiento"].ToString());
                    personasFisicasModel.UsuarioAgrega = Convert.ToInt32(dtbl.Rows[0]["UsuarioAgrega"].ToString());
                    personasFisicasModel.Activo = Convert.ToBoolean(dtbl.Rows[0]["Activo"].ToString());
                }
                return personasFisicasModel;
            }


        }
    }
}
